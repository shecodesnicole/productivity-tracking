using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Domain.Settings;
using Pd.Tasks.Application.Features.IAM.Models;
using Pd.Tasks.Application.Features.IAM.Persistance;
using Pd.Tasks.Application.Features.IAM.Requests;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using Pd.Tasks.Application.Features.UsersManagement.Persistence;
using Pd.Tasks.Application.Features.UsersManagement.Requests;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string InvalidUsernameOrPassword = "Invalid username or password";
        private const string DocumentType = "AuthenticationToken";
        private readonly IOptionsMonitor<AppSettings> appSettings;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IAuthenticationTokensRepository authenticationTokensRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IOptionsMonitor<AppSettings> appSettings,
            IUserRepository userRepository,
            IMapper mapper,
            IAuthenticationTokensRepository authenticationTokensRepository,
            IUnitOfWork unitOfWork,
            ILogger<AuthenticationService> logger)
        {
            this.appSettings = appSettings;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.authenticationTokensRepository = authenticationTokensRepository;
            this.unitOfWork = unitOfWork;
            _logger = logger;
            _logger.LogInformation("[AuthService] Constructor called");
        }

        public async Task<RequestResult<AuthenticationTokenModel>> LoginAsync(LoginCommand request)
        {
            // Check if user exists and is using the correct password
            var dbUser = await userRepository.GetUserAsync(s => s.Email == request.Email);

            if (dbUser == null)
            {
                return RequestResult.BadRequest<AuthenticationTokenModel>("No account found with this email address. Please register first.");
            }

            //if (!dbUser.IsActive)
            //{
            //    return RequestResult.BadRequest<AuthenticationTokenModel>("Your account has been deactivated. Please contact support for assistance.");
            //}

            //if (!dbUser.IsEmailVerified)
            //{
            //    return RequestResult.BadRequest<AuthenticationTokenModel>("Please verify your email address before logging in. Check your inbox for the verification link.");
            //}

            if (!HashingService.IsHashOf(dbUser.Password, request.Password))
            {
                return RequestResult.BadRequest<AuthenticationTokenModel>("Incorrect password. Please try again or reset your password.");
            }

            // Login
            var result = await AuthToken(dbUser);
            await authenticationTokensRepository.AddAuthenticationTokenAsync(result);

            return RequestResult.Success(result);

        }

        public async Task<RequestResult<AuthenticationTokenModel>> RegisterUserAsync(RegisterUserCommand request)
        {
            _logger.LogInformation("[AuthService] ===== RegisterUserAsync STARTED ===== Email: {Email}", request.Email);

            // Check if user exists
            var dbUser = await userRepository.GetUserAsync(s => s.Email == request.Email);

            if (dbUser != null)
            {
                _logger.LogWarning("[AuthService] Registration attempt for existing user: {Email}", request.Email);
                return RequestResult.BadRequest<AuthenticationTokenModel>("A user with this email address already exists. Please login or use a different email.");
            }

            _logger.LogInformation("[AuthService] Creating new user for {Email}...", request.Email);
            // Register user but do NOT login - require email verification AND admin approval
            var user = mapper.Map<UserModel>(request);
            user.Password = HashingService.Hash(request.Password);
          

            // Generate email verification token
            var verificationToken = GenerateSecureToken();
            user = await userRepository.AddUserAsync(user);
           
            await unitOfWork.SaveChangesAsync();
           
     
            return RequestResult.Success<AuthenticationTokenModel>(null);
        }

        private async Task<AuthenticationTokenModel> AuthToken(UserModel user)
        {
            if (string.IsNullOrEmpty(appSettings.CurrentValue.SecretKey))
            {
                throw new ArgumentNullException(nameof(appSettings.CurrentValue.SecretKey), "SecretKey cannot be null or empty.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.CurrentValue.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationTokenModel
            {
                User = user,
                AccessToken = tokenHandler.WriteToken(token),
                ExpiresAt = ((DateTimeOffset)tokenDescriptor.Expires).ToUnixTimeSeconds()
            };
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private string HashToken(string token)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(hashedBytes);
            }
        }

       
                
        private async Task<AuthenticationTokenModel> GenerateTokenAsync(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Services
{
    public class AuthenticationService : IAuthenticationService
    {
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
        }

        public async Task<RequestResult<AuthenticationTokenModel>> LoginAsync(LoginCommand request)
        {
            var dbUser = await userRepository.GetUserAsync(s => s.Email == request.Email);

            if (dbUser == null)
                return RequestResult.BadRequest<AuthenticationTokenModel>("No account found with this email address. Please register first.");

            if (!dbUser.IsActive)
                return RequestResult.BadRequest<AuthenticationTokenModel>("Your account has been deactivated. Please contact support for assistance.");

            if (!HashingService.IsHashOf(dbUser.Password, request.Password))
                return RequestResult.BadRequest<AuthenticationTokenModel>("Incorrect password. Please try again or reset your password.");

            var result = await AuthToken(dbUser);
            await authenticationTokensRepository.AddAuthenticationTokenAsync(result);
            return RequestResult.Success(result);
        }

        public async Task<RequestResult<AuthenticationTokenModel>> RegisterUserAsync(RegisterUserCommand request)
        {
            _logger.LogInformation("[AuthService] RegisterUserAsync started for {Email}", request.Email);

            var dbUser = await userRepository.GetUserAsync(s => s.Email == request.Email);
            if (dbUser != null)
                return RequestResult.BadRequest<AuthenticationTokenModel>("A user with this email address already exists. Please login or use a different email.");

            var user = mapper.Map<UserModel>(request);
            user.Password = HashingService.Hash(request.Password);
            user.IsActive = true;

            user = await userRepository.AddUserAsync(user);
            await unitOfWork.SaveChangesAsync();

            _logger.LogInformation("[AuthService] Registration complete for {Email}", request.Email);
            return RequestResult.Success<AuthenticationTokenModel>(null);
        }

        public async Task<RequestResult<bool>> ForgotPasswordAsync(ForgotPasswordCommand command)
        {
            try
            {
                var user = await userRepository.GetUserAsync(s => s.Email == command.Email);

                if (user == null)
                    return RequestResult.Success(true); // Don't reveal non-existence for security

                var token = GenerateSecureToken();
                user.PasswordResetToken = HashToken(token);
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);

                await unitOfWork.SaveChangesAsync();

                // Token is stored; in production, send it via email
                _logger.LogInformation("[AuthService] Password reset token generated for {Email}", command.Email);

                return RequestResult.Success(true);
            }
            catch (Exception ex)
            {
                return RequestResult.BadRequest<bool>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<RequestResult<bool>> ResetPasswordAsync(ResetPasswordCommand command)
        {
            try
            {
                var user = await userRepository.GetUserAsync(s => s.Email == command.Email);

                if (user == null)
                    return RequestResult.BadRequest<bool>("Invalid reset token.");

                if (string.IsNullOrEmpty(user.PasswordResetToken) ||
                    user.PasswordResetTokenExpiry == null ||
                    user.PasswordResetTokenExpiry < DateTime.UtcNow)
                    return RequestResult.BadRequest<bool>("Invalid or expired reset token.");

                var hashedToken = HashToken(command.Token);
                if (user.PasswordResetToken != hashedToken)
                    return RequestResult.BadRequest<bool>("Invalid reset token.");

                user.Password = HashingService.Hash(command.NewPassword);
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;

                await unitOfWork.SaveChangesAsync();

                return RequestResult.Success(true);
            }
            catch (Exception ex)
            {
                return RequestResult.BadRequest<bool>($"An error occurred: {ex.Message}");
            }
        }

        private async Task<AuthenticationTokenModel> AuthToken(UserModel user)
        {
            if (string.IsNullOrEmpty(appSettings.CurrentValue.SecretKey))
                throw new ArgumentNullException(nameof(appSettings.CurrentValue.SecretKey), "SecretKey cannot be null or empty.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.CurrentValue.SecretKey);
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

            // Use a new untracked object so EF Core doesn't persist the null password back to the DB
            var safeUser = new UserModel { Id = user.Id, Email = user.Email, UserRole = user.UserRole, IsActive = user.IsActive };
            return new AuthenticationTokenModel
            {
                User = safeUser,
                AccessToken = tokenHandler.WriteToken(token),
                ExpiresAt = ((DateTimeOffset)tokenDescriptor.Expires).ToUnixTimeSeconds()
            };
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

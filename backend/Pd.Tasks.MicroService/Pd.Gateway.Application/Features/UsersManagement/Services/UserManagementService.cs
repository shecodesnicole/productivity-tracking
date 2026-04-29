using AutoMapper;
using Microsoft.Extensions.Logging;
using Pd.Tasks.Application.Domain.Enums;
using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.IAM.Services;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using Pd.Tasks.Application.Features.UsersManagement.Persistence;
using Pd.Tasks.Application.Features.UsersManagement.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.UsersManagement.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UserManagementService> logger;

        public UserManagementService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserManagementService> logger)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<RequestResult<UserModel>> AddUserAsync(AddUserCommand userCommand, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserAsync(s => s.Email == userCommand.Email);

            var userInfo = new UserModel
            {
              
                Email = userCommand.Email,
                Password = HashingService.Hash(userCommand.Password)
            };

            if (user == null)
            {
                var result = await userRepository.AddUserAsync(userInfo);
                return new RequestResult<UserModel>
                {
                    IsSuccessful = true,
                    StatusCode = 200,
                    Data = result
                };
            }

            return new RequestResult<UserModel>
            {
                IsSuccessful = false,
                StatusCode = 400,
                ErrorMessage = "A user with this email already exists.",
                Data = new UserModel()
            };
        }

        public async Task<RequestResult<UserModel>> GetUserProfileAsync(GetUserProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserAsync(s => s.Email == command.Email);
            if (user == null)
                return RequestResult.NotFound<UserModel>("User not found");
            user.Password = null;
            return RequestResult.Success(user);
        }

        public async Task<RequestResult<UserModel>> UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserAsync(u => u.Email == command.Email);
            if (user == null)
                return RequestResult.NotFound<UserModel>("User not found");

            if (!string.IsNullOrEmpty(command.Password))
                user.Password = HashingService.Hash(command.Password);
            user.UserRole = command.UserRole;

            await userRepository.UpdateUserAsync(user, user);
            user.Password = null;
            return RequestResult.Success(user);
        }

        public async Task<RequestResult<UserModel[]>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var result = await userRepository.GetAllUsersAsync();
            foreach (var u in result) u.Password = null;
            return RequestResult.Success(result);
        }
    }
}


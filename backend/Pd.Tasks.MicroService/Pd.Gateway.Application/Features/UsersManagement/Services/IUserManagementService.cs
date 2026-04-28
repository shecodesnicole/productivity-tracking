using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using Pd.Tasks.Application.Features.UsersManagement.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.UsersManagement.Services
{
    public interface IUserManagementService
    {
        Task<RequestResult<UserModel>> AddUserAsync(AddUserCommand userCommand, CancellationToken cancellationToken);
        Task<RequestResult<UserModel>> GetUserProfileAsync(GetUserProfileCommand command, CancellationToken cancellationToken);
        Task<RequestResult<UserModel>> UpdateUserAsync(UpdateUserCommand command, CancellationToken cancellationToken);
        
        Task<RequestResult<UserModel[]>> GetAllUsersAsync(CancellationToken cancellationToken);
    }
}



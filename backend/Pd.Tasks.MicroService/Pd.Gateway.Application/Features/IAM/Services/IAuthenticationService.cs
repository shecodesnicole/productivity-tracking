using Pd.Tasks.Application.Domain.Requests;
using Pd.Tasks.Application.Features.IAM.Models;
using Pd.Tasks.Application.Features.IAM.Requests;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using Pd.Tasks.Application.Features.UsersManagement.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Services
{
    public interface IAuthenticationService
    {
        Task<RequestResult<AuthenticationTokenModel>> LoginAsync(LoginCommand request);
        Task<RequestResult<AuthenticationTokenModel>> RegisterUserAsync(RegisterUserCommand request);
    }
}
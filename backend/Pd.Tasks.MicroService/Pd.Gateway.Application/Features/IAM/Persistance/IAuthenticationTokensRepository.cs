using Pd.Tasks.Application.Features.IAM.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Persistance
{
    public interface IAuthenticationTokensRepository
    {
        Task<AuthenticationTokenModel> AddAuthenticationTokenAsync(AuthenticationTokenModel token);
    }
}

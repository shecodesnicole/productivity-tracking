using Pd.Tasks.Application.Features.IAM.Models;
using Pd.Tasks.Application.Features.IAM.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Persistence.Accessors
{
    public class AuthenticationTokensRepository : IAuthenticationTokensRepository
    {
        private readonly LocalDbContext localDbContext;

        public AuthenticationTokensRepository(LocalDbContext localDbContext)
        {
            this.localDbContext = localDbContext;
        }

        public Task<AuthenticationTokenModel> AddAuthenticationTokenAsync(AuthenticationTokenModel token)
        {
            // JWT tokens are stateless — no DB persistence required
            return Task.FromResult(token);
        }
    }
}

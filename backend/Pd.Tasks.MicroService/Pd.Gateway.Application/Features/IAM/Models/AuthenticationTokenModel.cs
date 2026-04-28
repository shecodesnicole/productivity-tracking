using Pd.Tasks.Application.Features.UsersManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Models
{
    public class AuthenticationTokenModel
    {
       
            public string Id { get; set; }

            public UserModel User { get; set; }

            public string AccessToken { get; set; }

            public long ExpiresAt { get; set; }
        }
    }


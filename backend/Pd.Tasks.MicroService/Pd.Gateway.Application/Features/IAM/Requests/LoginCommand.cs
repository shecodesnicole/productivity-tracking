using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.IAM.Requests
{
    public class LoginCommand
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

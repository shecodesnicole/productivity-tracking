using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.UsersManagement.Requests
{
    public class GetUserProfileCommand
    {
        public required string Email { get; set; }
    }
}

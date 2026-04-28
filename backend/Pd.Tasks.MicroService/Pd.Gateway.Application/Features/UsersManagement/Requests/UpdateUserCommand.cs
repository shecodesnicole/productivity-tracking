using Pd.Tasks.Application.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Pd.Tasks.Application.Features.UsersManagement.Requests
{
    public class UpdateUserCommand
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }



    }
}

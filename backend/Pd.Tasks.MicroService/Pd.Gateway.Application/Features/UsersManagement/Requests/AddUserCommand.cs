using Pd.Tasks.Application.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pd.Tasks.Application.Features.UsersManagement.Requests
{
    public class AddUserCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateAdded { get; set; }
        public UserRole UserRole { get; set; }
        
    }
}

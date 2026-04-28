using Pd.Tasks.Application.Features.UsersManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Pd.Tasks.Application.Features.UsersManagement.Persistence
{
    public interface IUserRepository
    {
        Task<UserModel?> GetUserAsync(Expression<Func<UserModel, bool>> predicate);
        Task<UserModel> AddUserAsync(UserModel user);
        Task<UserModel?> GetUserAsync(Expression<Func<UserModel, bool>> predicate, string[] includePaths);
        Task<UserModel> UpdateUserAsync(UserModel user, object userUpdate);
        Task<UserModel[]> GetUsersAsync(Expression<Func<UserModel, bool>> predicate);
        Task<UserModel[]> GetAllUsersAsync();
    }
}

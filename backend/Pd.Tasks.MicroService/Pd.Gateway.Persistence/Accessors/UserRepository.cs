using Microsoft.EntityFrameworkCore;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using Pd.Tasks.Application.Features.UsersManagement.Persistence;
using System.Linq.Expressions;

namespace Pd.Tasks.Persistence.Accessors
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext localDbContext;

        public UserRepository(LocalDbContext localDbContext)
        {
            this.localDbContext = localDbContext;
        }

        public async Task<UserModel?> GetUserAsync(Expression<Func<UserModel, bool>> predicate)
        {
            return await localDbContext.User.FirstOrDefaultAsync(predicate);
        }

        public async Task<UserModel?> GetUserAsync(Expression<Func<UserModel, bool>> predicate, string[] includePaths)
        {
            IQueryable<UserModel> query = localDbContext.User;
            foreach (var path in includePaths)
                query = query.Include(path);
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            if (string.IsNullOrEmpty(user.Id))
                user.Id = Guid.NewGuid().ToString();
            await localDbContext.AddAsync(user);
            return user;
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user, object userUpdate)
        {
            localDbContext.Entry(user).CurrentValues.SetValues(userUpdate);
            await localDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel[]> GetUsersAsync(Expression<Func<UserModel, bool>> predicate)
        {
            return await localDbContext.User.Where(predicate).ToArrayAsync();
        }

        public async Task<UserModel[]> GetAllUsersAsync()
        {
            return await localDbContext.User.ToArrayAsync();
        }
    }
}

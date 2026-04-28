using Pd.Tasks.Application.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pd.Tasks.Persistence
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly LocalDbContext _context;

        public UnitOfWork(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
        
            return await _context.SaveChangesAsync();
        }
    }
}

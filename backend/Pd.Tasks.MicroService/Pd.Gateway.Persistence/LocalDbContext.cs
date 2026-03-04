using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pd.Tasks.Application.Features.TaskManagement.Models;


namespace Pd.Tasks.Persistence
{
    public class LocalDbContext : DbContext
    {
        // Replace parameterless ctor with DI-based ctor
        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
        }
        
       

        public DbSet<TaskModel> Tasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>().HasKey(s => s.Id);

        }
        

        // Add this helper method to LocalDbContext
        private static string GeneratePbkdf2Hash(string password)
        {
            byte[] salt;
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt = new byte[16]);
            }
            
            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        // Add this method to your LocalDbContext class, after the existing SeedData method


       
    }
}

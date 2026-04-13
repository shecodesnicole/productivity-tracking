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

            modelBuilder.Entity<TaskModel>().HasData(

                new TaskModel
                {
                    Id = 11,
                    Title = "Fix authentication bug",
                    Description = "Users reporting issues with password reset flow",
                    Status = ProdashTaskStatus.ToDo,
                    CreatedAt = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc),
                    DueDate = new DateTime(2026, 4, 18, 12, 0, 0, DateTimeKind.Utc),
                    CompletedAt = null,
                    IsActive = true,
                    HoursWorked = 0

                },
                 new TaskModel
                 {
                     Id = 13,
                     Title = "Write unit tests",
                     Description = "Cover task service with xUnit tests",
                     Status = ProdashTaskStatus.Done,
                     CreatedAt = new DateTime(2026, 4, 9, 12, 0, 0, DateTimeKind.Utc),
                     DueDate = new DateTime(2026, 4, 16, 12, 0, 0, DateTimeKind.Utc),
                     CompletedAt = new DateTime(2026, 4, 11, 18, 0, 0, DateTimeKind.Utc),
                     IsActive = false,
                     HoursWorked = 6
                 },

                  new TaskModel
                  {
                      Id = 14,
                      Title = "Design login UI",
                      Description = "Create responsive login form for Prodash",
                      Status = ProdashTaskStatus.ToDo,
                      CreatedAt = new DateTime(2026, 4, 11, 14, 0, 0, DateTimeKind.Utc),
                      DueDate = new DateTime(2026, 4, 20, 12, 0, 0, DateTimeKind.Utc),
                      CompletedAt = null,
                      IsActive = true,
                      HoursWorked = 0
                  },

                    new TaskModel
                    {
                        Id = 15,
                        Title = "Database migration cleanup",
                        Description = "Reset EF Core migrations for fresh start",
                        Status = ProdashTaskStatus.InProgress,
                        CreatedAt = new DateTime(2026, 4, 8, 12, 0, 0, DateTimeKind.Utc),
                        DueDate = new DateTime(2026, 4, 15, 12, 0, 0, DateTimeKind.Utc),
                        CompletedAt = null,
                        IsActive = true,
                        HoursWorked = 2
                    },

                     new TaskModel
                     {
                         Id = 16,
                         Title = "Add task filtering",
                         Description = "Implement filtering by status in dashboard",
                         Status = ProdashTaskStatus.ToDo,
                         CreatedAt = new DateTime(2026, 4, 7, 12, 0, 0, DateTimeKind.Utc),
                         DueDate = new DateTime(2026, 4, 14, 12, 0, 0, DateTimeKind.Utc),
                         CompletedAt = null,
                         IsActive = true,
                         HoursWorked = 0
                     },
                      new TaskModel
                      {
                          Id = 17,
                          Title = "Setup CI/CD pipeline",
                          Description = "Configure GitHub Actions for Prodash",
                          Status = ProdashTaskStatus.InProgress,
                          CreatedAt = new DateTime(2026, 4, 6, 12, 0, 0, DateTimeKind.Utc),
                          DueDate = new DateTime(2026, 4, 13, 12, 0, 0, DateTimeKind.Utc),
                          CompletedAt = null,
                          IsActive = true,
                          HoursWorked = 4
                      },
                      new TaskModel
                      {
                          Id = 18,
                          Title = "Write API documentation",
                          Description = "Document endpoints for task management",
                          Status = ProdashTaskStatus.Done,
                          CreatedAt = new DateTime(2026, 4, 5, 12, 0, 0, DateTimeKind.Utc),
                          DueDate = new DateTime(2026, 4, 12, 12, 0, 0, DateTimeKind.Utc),
                          CompletedAt = new DateTime(2026, 4, 11, 18, 0, 0, DateTimeKind.Utc),
                          IsActive = false,
                          HoursWorked = 5
                      },
                      new TaskModel
                      {
                          Id = 19,
                          Title = "Add dark mode",
                          Description = "Implement dark theme toggle in UI",
                          Status = ProdashTaskStatus.ToDo,
                          CreatedAt = new DateTime(2026, 4, 4, 12, 0, 0, DateTimeKind.Utc),
                          DueDate = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc),
                          CompletedAt = null,
                          IsActive = true,
                          HoursWorked = 0
                      },
                       new TaskModel
                       {
                           Id = 20,
                           Title = "Optimize queries",
                           Description = "Improve EF Core query performance",
                           Status = ProdashTaskStatus.InProgress,
                           CreatedAt = new DateTime(2026, 4, 3, 12, 0, 0, DateTimeKind.Utc),
                           DueDate = new DateTime(2026, 4, 10, 12, 0, 0, DateTimeKind.Utc),
                           CompletedAt = null,
                           IsActive = true,
                           HoursWorked = 2
                       }
                       );
          
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

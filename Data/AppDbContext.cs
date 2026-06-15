using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Models;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;

namespace TraineeManagement1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LearningTask> LearningTasks{get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
               new User()
               {
                   Id = 1,
                   Username="admin",
                   Email="admin@gmail.com",
                   PasswordHash=BCrypt.Net.BCrypt.EnhancedHashPassword("password", workFactor: 12),
                   Role=RoleType.Admin,
                   CreatedDate=DateTime.UtcNow,
                   UpdatedDate=DateTime.UtcNow
               }
               );
        }

    }
}
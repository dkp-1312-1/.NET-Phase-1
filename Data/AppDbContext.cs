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
        public DbSet<LearningTask> LearningTasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
               new User()
               {
                   Id = 1,
                   Username = "admin",
                   Email = "admin@gmail.com",
                   PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("password", workFactor: 12),
                   Role = RoleType.Admin,
                   CreatedDate = DateTime.UtcNow,
                   UpdatedDate = DateTime.UtcNow
               }
               );

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Trainee)
                .WithMany(t => t.TaskAssignments)
                .HasForeignKey(ta => ta.TraineeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Mentor)
                .WithMany(m => m.TaskAssignments)
                .HasForeignKey(ta => ta.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.LearningTask)
                .WithMany(lt => lt.TaskAssignments)
                .HasForeignKey(ta => ta.LearningTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.TaskAssignment)
                .WithMany(ta => ta.Submissions)
                .HasForeignKey(s => s.TaskAssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Submission)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Mentor)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trainee>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<Mentor>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Models;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;
using TraineeManagement.Data.Enums;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TraineeManagement.Data.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private static string adminUserName = string.Empty;
        private static string adminEmail = string.Empty;
        private static string adminPassword = string.Empty;
        public AppDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), ".."))
                .AddJsonFile("appsettings.json")
                .Build();

            string? connectionString = configuration.GetConnectionString(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
        ? "DefaultConnection"
        : "LocalConnection");

            IConfigurationSection section = configuration.GetSection("AdminData");
            adminUserName = section["userName"];
            adminEmail = section["email"];
            adminPassword = section["password"];
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
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
        public DbSet<SubmissionFile> SubmissionFiles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProcessingJob> ProcessingJobs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            UserSeeding(modelBuilder);
            TaskAssignmentConstraint(modelBuilder);
            SubmissionConstraint(modelBuilder);
            ReviewConstraint(modelBuilder);
            SubmissionFileConstraint(modelBuilder);
            ProcessingJobConstraint(modelBuilder);
            TraineeEmailConstraint(modelBuilder);
            MentorEmailConstraint(modelBuilder);
            UserEmailConstraint(modelBuilder);
        }

        private void UserSeeding(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
               new User()
               {
                   Id = 1,
                   Username = adminUserName,
                   Email = adminEmail,
                   PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(adminPassword, workFactor: 12),
                   Role = RoleType.Admin,
                   CreatedDate = DateTime.UtcNow,
                   UpdatedDate = DateTime.UtcNow
               }
               );
        }
        private void TaskAssignmentConstraint(ModelBuilder modelBuilder)
        {
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
        }
        private void SubmissionConstraint(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.TaskAssignment)
                .WithMany(ta => ta.Submissions)
                .HasForeignKey(s => s.TaskAssignmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        private void ReviewConstraint(ModelBuilder modelBuilder)
        {
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
        }
        private void SubmissionFileConstraint(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubmissionFile>()
            .HasOne(r => r.Submission)
                .WithMany(s => s.SubmissionFiles)
                .HasForeignKey(r => r.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        private void ProcessingJobConstraint(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessingJob>()
            .HasOne(p => p.Submission)
                .WithMany(ps => ps.ProcessingJobs)
                .HasForeignKey(pj => pj.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        private void TraineeEmailConstraint(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainee>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
        private void MentorEmailConstraint(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mentor>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
        private void UserEmailConstraint(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }

    }
}

using System.ComponentModel.DataAnnotations; 
namespace TraineeManagement1.Models
{
    public enum RoleType
    {
        Admin,
        Mentor,
        Trainee
    }
    public class User
    {
        public required int Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public RoleType? Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
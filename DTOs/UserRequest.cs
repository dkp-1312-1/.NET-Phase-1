using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class UserRequest
    {
        [Required(ErrorMessage = "User name is required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Valid email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordHash is required")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Role[Admin, Mentor, Trainee] is required")]
        public RoleType Role { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Enums;
namespace TraineeManagement.Data.DTOs
{
    public class UserRequest
    {
        [Required(ErrorMessage = "User name is required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public required string Username { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Valid email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Role[Admin, Mentor, Trainee] is required")]
        public required RoleType Role { get; set; }
    }
}

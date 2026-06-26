using System.ComponentModel.DataAnnotations;

namespace TraineeManagement.Api.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
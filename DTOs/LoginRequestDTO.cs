using System.ComponentModel.DataAnnotations;

namespace TraineeManagement1.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
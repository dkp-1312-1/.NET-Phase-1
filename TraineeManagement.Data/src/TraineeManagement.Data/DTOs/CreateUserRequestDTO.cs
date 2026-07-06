using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.DTOs;
namespace TraineeManagement.Data.DTOs
{
    public class CreateUserRequestDTO :UserRequest
    {
        [Required(ErrorMessage = "PasswordHash is required")]
        public string? PasswordHash { get; set; }
    }

}

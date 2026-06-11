using System.ComponentModel.DataAnnotations;
using TraineeManagement1.DTOs;
namespace TraineeManagement1.DTOs
{
    public class CreateUserRequestDTO :UserRequest
    {
        [Required(ErrorMessage = "PasswordHash is required")]
        public string PasswordHash { get; set; }
    }

}
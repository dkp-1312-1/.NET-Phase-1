using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.DTOs
{
    public class CreateUserRequestDTO :UserRequest
    {
        [Required(ErrorMessage = "PasswordHash is required")]
        public string? PasswordHash { get; set; }
    }

}
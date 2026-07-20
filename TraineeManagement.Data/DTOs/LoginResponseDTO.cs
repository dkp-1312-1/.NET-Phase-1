using System.ComponentModel.DataAnnotations;
namespace TraineeManagement.Data.DTOs
{
    public class LoginResponseDTO
    {
        public string? Token {get;set;}
        public int ExpiresIn{get;set;}
        public UserInfoDTO? User {get;set;}
    }
}

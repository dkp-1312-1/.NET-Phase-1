using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.DTOs
{
    public class LoginResponseDTO
    {
        public string Token{get;set;}
        public int ExpiresInMinutes{get;set;}
        public UserInfoDTO User{get;set;}
    }
}
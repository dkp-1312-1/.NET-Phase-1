using System.ComponentModel.DataAnnotations;
using TraineeManagement1.DTOs;
namespace TraineeManagement1.DTOs
{
    public class LoginResponseDTO
    {
        public string Token;
        public int ExpiresIn;
        public UserInfoDTO User;
    }
}
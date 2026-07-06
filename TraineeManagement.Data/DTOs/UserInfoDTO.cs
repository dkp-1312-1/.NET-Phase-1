using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Enums;
namespace TraineeManagement.Data.DTOs
{
    public class UserInfoDTO
    {
        public int Id{get;set;}
        public string? Username {get;set;}
        public RoleType Role{get;set;}
    }
}

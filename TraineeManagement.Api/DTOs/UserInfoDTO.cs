using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.DTOs
{
    public class UserInfoDTO
    {
        public int Id{get;set;}
        public string Username {get;set;}
        public RoleType Role{get;set;}
    }
}
using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class UserInfoDTO
    {
        public int Id{get;set;}
        public string Username {get;set;}
        public RoleType Role{get;set;}
    }
}
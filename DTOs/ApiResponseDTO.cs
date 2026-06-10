using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class ApiResponseDTO
    {
        public TraineeResponseDTO Data{get;set;}
        public bool Success {get;set;}
    }
}
using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class ApiResponseDTO<T>
    {
        public T Data{get;set;}
        public bool Success {get;set;}
    }
}
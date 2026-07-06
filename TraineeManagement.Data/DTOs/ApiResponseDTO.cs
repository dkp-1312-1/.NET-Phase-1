using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Models;
namespace TraineeManagement.Data.DTOs
{
    public class ApiResponseDTO<T>
    {
        public T? Data{get;set;}
        public bool Success {get;set;}
    }
}

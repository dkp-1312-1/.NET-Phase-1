using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Models;
namespace TraineeManagement.Api.DTOs
{
    public class ApiResponseDTO<T>
    {
        public T Data{get;set;}
        public bool Success {get;set;}
    }
}
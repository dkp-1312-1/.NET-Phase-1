using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Models;
namespace TraineeManagement.Data.DTOs
{
    public class Errors
    {
        public string? Field{get;set;}
        public string? Message{get;set;}
    }
    public class ApiErrorResponseDTO
    {
        public IEnumerable<Errors>? Errors { get; set; }
        public bool Success {get;set;}
    }
}

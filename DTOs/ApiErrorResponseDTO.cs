using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;
namespace TraineeManagement1.DTOs
{
    public class Errors
    {
        public string? Field{get;set;}
        public string? Message{get;set;}
    }
    public class ApiErrorResponseDTO
    {
        public IEnumerable<Errors> Errors { get; set; }
        public bool Success {get;set;}
    }
}
using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Models;
namespace TraineeManagement.Api.DTOs
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
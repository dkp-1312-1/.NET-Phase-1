using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.Models
{
    public class Trainee
    {
        public required int id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? TechStack { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
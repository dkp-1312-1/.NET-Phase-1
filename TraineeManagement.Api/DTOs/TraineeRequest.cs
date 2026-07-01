using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.DTOs
{
    public class TraineeRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Valid email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "TechStack is required")]
        public string? TechStack { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public TraineeStatusType Status { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TraineeManagement1.DTOs
{
    public class CreateTraineeRequestDTO
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
        public string? Status { get; set; }
    }
    public class UpdateTraineeRequestDTO
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
        [Required(ErrorMessage ="Status is required")]
        public string? Status { get; set; }
    }
    public class TraineeResponseDTO
    {
        public int? id{get;set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? TechStack { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

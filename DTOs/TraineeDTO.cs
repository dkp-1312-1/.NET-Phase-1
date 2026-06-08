using System.ComponentModel.DataAnnotations;

namespace TraineeManagement1.DTOs
{
    public class CreateTraineeRequestDTO
    {
        public required int id { get; set; }
        [Required(ErrorMessage = "First Name Required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "TechStack Required")]
        public string? TechStack { get; set; }
        [Required(ErrorMessage = "Status Required")]
        public string? Status { get; set; }
    }
    public class UpdateTraineeRequestDTO
    {
        public required int id { get; set; }
        [Required(ErrorMessage = "First Name Required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Required")]
        [StringLength(50, ErrorMessage = "Max 50 Characters")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "TechStack Required")]
        public string? TechStack { get; set; }
        [Required(ErrorMessage ="Status Requiured")]
        public string? Status { get; set; }
    }
    public class TraineeResponseDTO
    {
        public int ? id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? TechStack { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

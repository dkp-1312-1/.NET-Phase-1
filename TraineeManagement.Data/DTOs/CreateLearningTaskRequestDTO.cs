using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
namespace TraineeManagement.Data.DTOs
{
    public class CreateLearningTaskRequestDTO   
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Max 100 Characters")]
        public string? Title { get; set; }
 
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
 
        [Required(ErrorMessage = "Expected tech stack is required")]
        public string? ExpectedTechStack { get; set; }
 
        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }
 
        [Required(ErrorMessage = "Status is required")]
        public LTStatusType Status { get; set; }
    }
}

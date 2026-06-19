using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.DTOs
{
    public class CreateLearningTaskRequestDTO   
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Max 100 Characters")]
        public string Title { get; set; }
 
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
 
        [Required(ErrorMessage = "Expected tech stack is required")]
        public string ExpectedTechStack { get; set; }
 
        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }
 
        [Required(ErrorMessage = "Status is required")]
        public LTStatusType Status { get; set; }
    }
}
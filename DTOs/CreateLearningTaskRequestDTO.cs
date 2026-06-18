using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;

namespace TraineeManagement1.DTOs
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
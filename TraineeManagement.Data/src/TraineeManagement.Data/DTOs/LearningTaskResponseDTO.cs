using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Enums;
namespace TraineeManagement.Data.DTOs
{
    public class LearningTaskResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ExpectedTechStack { get; set; }
        public DateTime DueDate { get; set; }
        public LTStatusType Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
 

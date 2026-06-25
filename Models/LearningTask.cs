using TraineeManagement.Api.Enums;
using TraineeManagement.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Api.Models
{
    public class LearningTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExpectedTechStack { get; set; }
        public DateTime DueDate { get; set; }
        public LTStatusType Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }

        public LearningTask(CreateLearningTaskRequestDTO request)
        {
            Title = request.Title;
            Description = request.Description;
            ExpectedTechStack = request.ExpectedTechStack;
            DueDate = request.DueDate;
            Status = request.Status;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
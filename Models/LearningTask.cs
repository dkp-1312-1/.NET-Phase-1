
namespace TraineeManagement1.Models
{
    public enum LTStatusType
    {
        Draft,
        Published,
        Closed
    }
    public class LearningTask
    {
        public required int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ExpectedTechStack { get; set; }
        public DateTime DueDate { get; set; }
        public LTStatusType Status { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }
    }
}
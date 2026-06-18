namespace TraineeManagement1.Resources
{
    public class SharedResource
    {
        public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
        public const string Unauthorized = "Invalid username or password.";
        public const string InvalidToken = "The provided authentication token is invalid.";

    
        public static string TraineeNotFound(int id) => $"Trainee with id {id} was not be found.";
        public static string MentorNotFound(int id) => $"Mentor with id {id} was not found.";
        public static string AssignmentNotFound(int id) => $"Task Assignment with id {id} was not found.";
        public static string SubmissionNotFound(int id) => $"Submission with id {id} was not found.";
        public static string ReviewNotFound(int id) => $"Review with id {id} was not found.";
        public static string TaskNotFound(int id) => $"Learning Task with id {id} was not found.";
    }
}
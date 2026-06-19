namespace TraineeManagement.Api.Resources
{
    public class StringConstants
    {
        public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
        public const string Unauthorized = "Invalid username or password.";
        public const string InvalidToken = "The provided authentication token is invalid.";
        public const string JWTUnauthorized="Unauthorized, Please Login.";
        public const string InternalError="An unexpected error occurred. Please try again later.";


        public const string mysqlEmail="This email address is already registered.";
        public const string mysqlUsername= "Username already exists.";
        public const string mysqlDuplicate="A record with these details already exists.";
        public const string mysql1451="Cannot delete or update because of related data. Please remove related data first or change reference";
        public const string mysql1452="Related data not found, Please ensure referenced data exists..";


        public static string TraineeNotFound(int id) => $"Trainee with id {id} was not be found.";
        public static string MentorNotFound(int id) => $"Mentor with id {id} was not found.";
        public static string AssignmentNotFound(int id) => $"Task Assignment with id {id} was not found.";
        public static string SubmissionNotFound(int id) => $"Submission with id {id} was not found.";
        public static string ReviewNotFound(int id) => $"Review with id {id} was not found.";
        public static string TaskNotFound(int id) => $"Learning Task with id {id} was not found.";
    }
}
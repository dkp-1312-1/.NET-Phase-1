namespace TraineeManagement.Api.Resources
{
    public class StringConstants
    {
        public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
        public const string Unauthorized = "Invalid username or password.";
        public const string InvalidToken = "The provided authentication token is invalid.";
        public const string JWTUnauthorized = "Unauthorized, Please Login.";
        public const string InternalError = "An unexpected error occurred. Please try again later.";
        public const string InvalidFileType = "Invalid File Type.";

        public const string mysqlEmail = "This email address is already registered.";
        public const string mysqlUsername = "Username already exists.";
        public const string mysqlDuplicate = "A record with these details already exists.";
        public const string mysql1451 = "Cannot delete or update because of related data. Please remove related data first or change reference";
        public const string mysql1452 = "Related data not found, Please ensure referenced data exists..";

        public const string fileEmpty = "File is empty.";
        public const string noAccessDownload = "You havenot access to download file.";
        public const string fileNotFound = "The physical file could not be found.";
        public const string deleteFileError = "Error deleting the file from storage.";

        public const string dueDateError = "DueDate should not be before AssignedDate.";
        public const string RedisUnavailable = "Redis is unavailable. Try to get data from MySQL...";
        public const string RabbitUnavailable = "RabbitMQ is unavailable. Try again later...";

        public const string LogFilePath = "/app/logs/app-.txt";
        public const string LogOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}";



        public static string TraineeNotFound(int id) => $"Trainee with id {id} was not be found.";
        public static string MentorNotFound(int id) => $"Mentor with id {id} was not found.";
        public static string AssignmentNotFound(int id) => $"Task Assignment with id {id} was not found.";
        public static string SubmissionNotFound(int id) => $"Submission with id {id} was not found.";
        public static string ReviewNotFound(int id) => $"Review with id {id} was not found.";
        public static string TaskNotFound(int id) => $"Learning Task with id {id} was not found.";
        public static string SubmissionFileNotFound(int id) => $"File with id {id} was not found.";
        public static string JobNotFound(string id) => $"Job with tracking id {id} was not found.";
        public static string fileSizeExceed(int size) => $"File exceeds the {size}MB limit.";
        public static string trainee(int id) => $"trainee:{id}";
        public static string taskAssignment(int id) => $"taskAssignment:{id}";
        public static string submission(int id) => $"submission:{id}";


    }
}
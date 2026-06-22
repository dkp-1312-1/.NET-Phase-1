# .NET Phase - 1
# Install Packages:

dotnet add package NSwag.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNETCore

# Project Name =TraineeManagement1
Technology Used =Asp.NET Web API, Service Layer, DTOs,JWT, EFCore.InMemory Databases, MySql.EntityFrameworkCore,Microsoft.EntityFrameworkCore.Design, 


>Packages:
BCrypt.Net-Next
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.AspNetCore.OpenApi
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.InMemory
Microsoft.NETCore.App
MySql.EntityFrameworkCore
NSwag.AspNetCore
>How to Run =Install zip file ,install Packages given above ,
>Comand to Run: dotnet run --launch-profile https


# MySql Database Setup Steps

>Read the database connection string from appsettings.json and append it in starts:  
{  
"ConnectionStrings":  
  {  
    "DefaultConnection": "server=localhost;port=3306;database=trainee_management_db;user=root;password=your_password;"  
  }  
} 

>Change Program.cs file with addding given code snipper before  var app=builder.build(); 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContext<AppDbContext>(options => 
       options.UseMySQL(connectionString)); 

# Run given command in ubuntu for creating migration: It creates migration folder and 3 files. 
dotnet ef migrations add InitialCreate 

>Run given command in ubuntu for updating and connecting to databases. 
dotnet ef database update 
 
# Login credentials for testing
user=admin
password=password

# JWT usage instructions
In .env File, You can change JWT variables like Key,Issuer,Audience,ExpiryIn

# API List =
GET /api/health

GET /api/trainees

GET /api/trainees/{id}

POST /api/trainees
>Request: 
{
 "firstName": "Amit",
 "lastName": "Sharma",
 "email": "amit.sharma@training.com",
 "techStack": "HTML, CSS, JavaScript",
 "status": "Active"
}

>Response: 
{
  "id": 1,
  "firstName": "Amit",
  "lastName": "Sharma",
  "email": "amit.sharma@training.com",
  "techStack": "HTML, CSS, JavaScript",
  "status": "Active",
  "createdDate": "2026-06-08T13:59:58.1051787Z",
  "updatedDate": "2026-06-08T13:59:58.1051937Z"
}

PUT /api/trainees/{id}

DELETE /api/trainees/{id}

GET /api/trainees?pageNumber=1&pageSize=10&search=amit&status=Active
>Response:
{
 "pageNumber": 1,
 "pageSize": 10,
 "totalRecords": 25,
 "data": []
}

>User Login 
POST /api/auth/login

>Request:
{
 "username": "admin",
 "password": "Admin@123"
}
>Response:
{
 "token": "jwt-token-value",
 "expiresIn": 3600,
 "user": {
 "id": 1,
 "username": "admin",
 "role": "Admin"
 }
}

>Mentor:
GET /api/mentors?pageNumber=1&pageSize=10&search=amit&status=Active
GET /api/mentors/{id}
POST /api/mentors
PUT /api/mentors/{id}
DELETE /api/mentors/{id}

>Learning Task:
GET /api/learning-tasks
GET /api/learning-tasks/{id}
POST /api/learning-tasks
PUT /api/learning-tasks/{id}
DELETE /api/learning-tasks/{id}

>Task Assignment:
POST /api/task-assignments
GET /api/task-assignments
GET /api/task-assignments/{id} 
PUT /api/task-assignments/{id}/status

>Submission:
POST /api/submissions
GET /api/submissions
GET /api/submissions/{id}

>Review:
POST /api/reviews
GET /api/reviews
GET /api/reviews/{id}

# Known Limitations
-Authorization for role based

# Security Checklist:


# Next improvement areas:
-Role based access



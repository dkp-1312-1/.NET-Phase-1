#.NET Phase - 1
Install Packages:

dotnet add package NSwag.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNETCore



Api :

GET /api/health

GET /api/trainees

GET /api/trainees/{id}

POST /api/trainees

PUT /api/trainees/{id}

DELETE /api/trainees/{id}

GET /api/trainees?search=amit

Post Request Example:
{
 "firstName": "Amit",
 "lastName": "Sharma",
 "email": "amit.sharma@training.com",
 "techStack": "HTML, CSS, JavaScript",
 "status": "Active"
}


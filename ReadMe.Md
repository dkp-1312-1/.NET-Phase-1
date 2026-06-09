#.NET Phase - 1
Install Packages:

dotnet add package NSwag.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNETCore

Project Name =TraineeManagement1
Technology Used =Asp.NET Web API, Service Layer, DTOs, EFCore.InMemory Databases  
How to Run =Install zip file ,install Packages given above 
API List =
GET /api/health

GET /api/trainees

GET /api/trainees/{id}

POST /api/trainees

PUT /api/trainees/{id}

DELETE /api/trainees/{id}

GET /api/trainees?search=amit


Sample Request JSON 
{
 "firstName": "Amit",
 "lastName": "Sharma",
 "email": "amit.sharma@training.com",
 "techStack": "HTML, CSS, JavaScript",
 "status": "Active"
}


Sample Response JSON 

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


Known Limitations
-It currently uses In Memory database which holds data temporarily. It lacks real Sql/No Sql database connection.
-Absence of authentication and authorisation







{
    data: {
        trainees: [
            {
                "id": 1,
                "firstName": "Amit",
                "lastName": "Sharma",
                "email": "amit.sharma@training.com",
                "techStack": "HTML, CSS, JavaScript",
                "status": "Active",
                "createdDate": "2026-06-09T07:13:56.8274919Z",
                "updatedDate": "2026-06-09T07:13:56.8275169Z"
            },
            {
                "id": 2,
                "firstName": "Amit1",
                "lastName": "Sharma",
                "email": "amit.sharma@training.com",
                "techStack": "HTML, CSS, JavaScript",
                "status": "Active",
                "createdDate": "2026-06-09T07:18:00.4040465Z",
                "updatedDate": "2026-06-09T07:18:00.4040466Z"
            }
        ],
        total: 2
    }
    success: true,
}

{
    data: {
        trainees: [],
        total: 0
    }
    success: true,
}

{
    Name:
    email:
    techStack
    createedate:
    status:
}

{
    "errors": {
 "firstName": ["First name is required"],
 "email": ["Valid email is required"]
    }
    success: false,
}

{
    data: {..}
    success: true,
}

try catch
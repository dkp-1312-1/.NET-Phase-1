# .NET Phase - 1
# Install Packages:

dotnet add package NSwag.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNETCore

# Project Name =TraineeManagement1
Technology Used =Asp.NET Web API, Service Layer, DTOs, EFCore.InMemory Databases, MySql.EntityFrameworkCore,Microsoft.EntityFrameworkCore.Design, 
>How to Run =Install zip file ,install Packages given above ,
>Comand to Run: dotnet run --launch-profile https


# API List =
GET /api/health

GET /api/trainees

GET /api/trainees/{id}

POST /api/trainees

PUT /api/trainees/{id}

DELETE /api/trainees/{id}

GET /api/trainees?search=amit


# Sample Request JSON 
{
 "firstName": "Amit",
 "lastName": "Sharma",
 "email": "amit.sharma@training.com",
 "techStack": "HTML, CSS, JavaScript",
 "status": "Active"
}


# Sample Response JSON 

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


# Known Limitations
-It currently uses In Memory database which holds data temporarily. It lacks real Sql/No Sql database connection.
-Absence of authentication and authorisation



# Database Setup Steps

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

>Run given command in ubuntu for creating migration: It creates migration folder and 3 files. 
dotnet ef migrations add InitialCreate 

>Run given command in ubuntu for updating and connecting to databases. 
dotnet ef database update 
 

 
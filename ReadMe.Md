# .NET Phase - 1
# Install Packages:

dotnet add package NSwag.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.AspNETCore

# Project Name = TraineeManagement1
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
RabbitMQ.Client
MySqlConnector
Microsoft.Extensions.Caching.StackExchangeRedis
Microsoft.Extensions.Http.Resilience
>How to Run =Install zip file ,install Packages given above ,
>Comand to Run: dotnet run --launch-profile https in local 
>docker compose up --build
>docker compose ps //To see working Container
>docker compose down //To inactive Container

# Configuration
>./AppSetting.json.example File 
{
  "ConnectionStrings": {
    "DefaultConnection": "",
    "LocalConnection": "",
    "RedisConnection": ""
  },
  "RabbitMQ": {
    "HostName": "",
    "Port": ,
    "UserName": "",
    "Password": "",
    "VirtualHost": ""
  },
  "AllowedHosts": "",
  "Jwt": {
    "Key": "",
    "Issuer": "",
    "Audience": "",
    "ExpiryMinutes": 
  },
  "FileStorage": {
    "RootPath": "",
    "FileSizeLimit": ,
    "AllowedExtensions": [
      "",
      "",
      ""
    ]
  },
  "Redis": {
    "CacheTimeLimit": 
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "",
      "Override": {
        "Microsoft": "",
        "Microsoft.Hosting.Lifetime": "",
        "Microsoft.AspNetCore.Hosting": "",
        "Microsoft.EntityFrameworkCore.Database": "",
        "System.Net.Http.HttpClient": "",
        "Microsoft.AspNetCore.Hosting.Diagnostics": "",
        "Microsoft.AspNetCore.Routing.EndpointMiddleware": "",
        "System": "",
        "MySql": ""
      }
    },
    "WriteTo": [
      {
        "Name": ""
      }
    ]
  },
  "AdminData":{
    "userName":"",
    "email":"",
    "password":""
  }
}

# Design Decisions

Cache (Redis)
- Keys: trainee:{id}, submission:{id}, taskAssignment:{id}
- Pattern: cache-aside — read tries Redis first, falls back to MySQL on a miss, then repopulates Redis.
- TTL: 30 minutes, 
- Invalidation: Update/Delete operations remove the cache key before writing to MySQL
- Resilience: all cache operations (Get/Set/Remove) are logged and the API transparently falls back to MySQL 

Queue Settings
- Queue submission-processing: durable=true; 
messages are published with Persistent=true 
- Dead-lettering: the queue is declared with x-dead-letter-exchange / x-dead-letter-routing-key pointing at the dead-letter-exchange

Retries & Idempotency
- the worker retries the RabbitMQ connection up to 10 times with a fixed 5s delay before giving up.
- After Attempts > 3: the job is marked Failed  
- Idempotency key: If a message is redelivered, the worker looks : if it's already Completed or Failed, the worker Acks immediately without reprocessing.
- File integrity: the SHA-256 checksum computed at upload time is recomputed by the worker before marking a job Completed

Failure Behaviour
- Redis down → API keeps serving from MySQL 
- RabbitMQ publish failure → the upload API call returns 503 Service Unavailable to the client up front

# MySql Database Setup Steps

>Read the database connection string from appsettings.json and append it in starts:  
{  
"ConnectionStrings":  
  {  
    "DefaultConnection": ""  
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
>user=
>password=

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
 "username": "",
 "password": ""
}
>Response:
{
 "token": "",
 "expiresIn": ,
 "user": {
 "id": ,
 "username": "",
 "role": ""
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
-Status Changes of inactive or active or other types
-Review of Submission must done by only mentor which was assigned

# Security Checklist:


# Next improvement areas:
-Role based access


# Challenges:
-
-For referencing in worker project,we have to give default path for Getting Files for Submission Files 
-builder.Logging.ClearProviders();builder.Logging.AddConsole(); Added This command to Enable it to work BeginScope
-add "/vhostname" in rabiitmq Api name to add its virtualHostName 
-configure redis in Worker YML File
-Port and HostName Differ in docker-compose.yml and appSettting.json
-give different name for storage Directory in appSetting.js and include same in docker-compose.yml file.
-Configure connectionString in yml to override it from appSetting.json in Docker Container 
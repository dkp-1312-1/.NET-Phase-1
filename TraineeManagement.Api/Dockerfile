# Base runtime image
FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY *.sln ./
COPY TraineeManagement.Api/*.csproj TraineeManagement.Api/
COPY TrainingDirectory.Api/*.csproj TrainingDirectory.Api/
COPY SubmissionProcessor.Worker/*.csproj SubmissionProcessor.Worker/
COPY NuGet.Config ./

RUN dotnet restore "./TraineeManagement.Api/TraineeManagement.Api.csproj" --configfile NuGet.Config


COPY . .
RUN dotnet build TraineeManagement.Api/TraineeManagement.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish TraineeManagement.Api/TraineeManagement.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TraineeManagement.Api.dll"]

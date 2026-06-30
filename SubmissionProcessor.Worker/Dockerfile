FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app

FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY *.sln ./
COPY TraineeManagement.Api/*.csproj TraineeManagement.Api/
COPY TrainingDirectory.Api/*.csproj TrainingDirectory.Api/
COPY SubmissionProcessor.Worker/*.csproj SubmissionProcessor.Worker/
COPY NuGet.Config ./

RUN dotnet restore "./SubmissionProcessor.Worker/SubmissionProcessor.Worker.csproj" --configfile NuGet.Config

COPY . .
RUN dotnet build SubmissionProcessor.Worker/SubmissionProcessor.Worker.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish SubmissionProcessor.Worker/SubmissionProcessor.Worker.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubmissionProcessor.Worker.dll"]


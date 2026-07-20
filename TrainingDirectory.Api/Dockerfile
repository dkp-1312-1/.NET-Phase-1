FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM docker-registry-002.zeuslearning.com/zeuslearning/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY *.sln ./
COPY TraineeManagement.Api/*.csproj TraineeManagement.Api/
COPY TrainingDirectory.Api/*.csproj TrainingDirectory.Api/
COPY SubmissionProcessor.Worker/*.csproj SubmissionProcessor.Worker/
COPY NuGet.Config ./

RUN dotnet restore "./TrainingDirectory.Api/TrainingDirectory.Api.csproj" --configfile NuGet.Config

COPY . .
RUN dotnet build TrainingDirectory.Api/TrainingDirectory.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish TrainingDirectory.Api/TrainingDirectory.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainingDirectory.Api.dll"]
# syntax=docker/dockerfile:1
### FIRST STEP ###
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:~/.dotnet/tools"

# Copy csproj and restore as distinct layers
COPY FeeApi/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY FeeApi/. ./


# Libs already in FeeApi.csproj
#RUN dotnet add package Microsoft.AspNet.WebApi.Client
#RUN dotnet add package Microsoft.EntityFrameworkCore.Design
#RUN dotnet add package Microsoft.EntityFrameworkCore.SqlServer
#RUN dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
#RUN dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Creating Database tables
RUN dotnet ef migrations add PostgreInitial

# Updates on DB are being done in Program.cs automatically
#RUN dotnet ef database update

# Creating production files
RUN dotnet publish -c Release -o out


### SECOND STEP ###
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
#ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app

COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "FeeApi.dll"]
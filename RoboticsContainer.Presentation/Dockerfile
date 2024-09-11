# Use the official .NET SDK as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["Containers/RoboticsContainer/RoboticsContainer.csproj", "RoboticsContainer/"]
RUN dotnet restore "RoboticsContainer/RoboticsContainer.csproj"

# Copy the entire project
COPY Containers/RoboticsContainer/. RoboticsContainer/

# Set the working directory inside the container
WORKDIR /src/RoboticsContainer

# Build the project
RUN dotnet build -c Release -o /app/build

# Publish the project
RUN dotnet publish -c Release -o /app/publish

# Use a smaller runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build-env /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "RoboticsContainer.dll"]

# Use the official .NET 6.0 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /source

# Copy the project files into the container
COPY . .

# Restore dependencies and build the application
RUN dotnet restore "./Server-cloudata/Server-cloudata.csproj" -- disable-parallel
RUN dotnet publish "./Server-cloudata/Server-cloudata.csproj" -c release -o /app --no-restore

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Expose the port that the application will listen on
EXPOSE 5001

# Run the application when the container starts
CMD ["dotnet", "Server-cloudata.dll"]
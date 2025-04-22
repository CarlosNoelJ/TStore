# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file(s) and restore as distinct layers.
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code and build the application.
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
# Copy the published output from the build stage
COPY --from=build /app/publish .

# Set the environment variable for ASP.NET Core to listen on port 80.
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Develpoment

# Expose port 80 to the outside world.
EXPOSE 80

# Start the application.
ENTRYPOINT ["dotnet", "Berx3.API.dll"]

# Stage 1: Build the application
# Use the Red Hat UBI SDK image for building
FROM registry.access.redhat.com/ubi8/dotnet-90:9.0-1758501291 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files and restore dependencies
# The *.csproj is copied first to leverage Docker's layer caching
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Publish the application for a release build
RUN dotnet publish -c Release -o out

# Stage 2: Create the final runtime image
# Use the Red Hat UBI runtime image for the final, smaller image
FROM registry.redhat.io/ubi8/dotnet-90-runtime:9.0.9-1758211314

# Set the working directory
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out .

# Expose the port the application will run on
# This is a standard practice for containerized applications
EXPOSE 8080

# Set the entrypoint to run the application
# The application name should be the same as your .csproj file
ENTRYPOINT ["dotnet", "TodoApi.dll"]
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./

# Copy solution file
COPY ChatApp.sln ./

# Copy all project files
COPY ChatRoomHub/*.csproj ChatRoomHub/
COPY Chat.Application/*.csproj Chat.Application/
COPY Chat.Contracts/*.csproj Chat.Contracts/
COPY Chat.Domain/*.csproj Chat.Domain/
COPY Chat.Infrastructure/*.csproj Chat.Infrastructure/

# Restore dependencies
RUN dotnet restore ChatApp.sln

# Copy everything else
COPY . .

# Build the application
WORKDIR /src/ChatRoomHub
RUN dotnet build -c Release -o /app/build --no-restore

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "Chat.Api.dll"]
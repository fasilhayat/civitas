# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7080
EXPOSE 7081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only .csproj and related props/targets files first to cache restore layer
COPY Civitas.Api/*.csproj ./Civitas.Api/

# Restore as a separate layer to benefit from caching
RUN dotnet restore ./Civitas.Api/Civitas.Api.csproj

# Copy the rest of the source code
COPY . .

# Build the project
WORKDIR /src/Civitas.Api
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build --no-restore

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Optional: avoid running as root by setting a secure default
# USER appuser (if created via Dockerfile or image)

COPY ./Civitas.Api/civitas.pfx /app/civitas.pfx
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Civitas.Api.dll"]
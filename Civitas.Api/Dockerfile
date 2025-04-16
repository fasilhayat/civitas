# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

COPY ./Civitas.Api/civitas.pfx /app/civitas.pfx

EXPOSE 7080
EXPOSE 7081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy just the .csproj file from the correct path
COPY Civitas.Api/Civitas.Api.csproj ./Civitas.Api/
RUN dotnet restore ./Civitas.Api/Civitas.Api.csproj

# Copy the rest of the code
COPY . .

WORKDIR /src/Civitas.Api
RUN dotnet build "Civitas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Civitas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Civitas.Api.dll"]

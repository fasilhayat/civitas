# ----------------------
# Build Stage
# ----------------------
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release

# Create and switch to working directory
WORKDIR /src

# Copy nuget config and local feed before solution restore
COPY nuget.config ./nuget.config
COPY local-nuget-feed/ ./local-nuget-feed/

# Ensure project directory structure exists before copying
RUN mkdir -p Civitas.Api

# Copy .csproj file explicitly into subdirectory
COPY Civitas.Api/*.csproj Civitas.Api/

# Disable signature verification and restore dependencies
ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
RUN dotnet restore -a $TARGETARCH --configfile nuget.config ./Civitas.Api/Civitas.Api.csproj

# Copy the rest of the source files
COPY . .

# Set working directory to project and publish
WORKDIR /src/Civitas.Api
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish -a $TARGETARCH --no-restore /p:UseAppHost=false

# ----------------------
# Runtime Stage
# ----------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the certificate and published output
COPY ./Civitas.Api/civitas.pfx ./civitas.pfx
COPY --from=build /app/publish .

# Expose HTTP and HTTPS ports
EXPOSE 7080
EXPOSE 7081

# Run the application
ENTRYPOINT ["dotnet", "Civitas.Api.dll"]

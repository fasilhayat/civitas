# Build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Civitas.Api/*.csproj ./Civitas.Api/
ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
RUN dotnet restore -a $TARGETARCH ./Civitas.Api/Civitas.Api.csproj

COPY . .
WORKDIR /src/Civitas.Api
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish -a $TARGETARCH --no-restore /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY ./Civitas.Api/civitas.pfx /app/civitas.pfx
COPY --from=build /app/publish .

EXPOSE 7080
EXPOSE 7081
ENTRYPOINT ["dotnet", "Civitas.Api.dll"]

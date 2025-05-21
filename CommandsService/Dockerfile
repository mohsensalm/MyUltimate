# Stage 1: Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ✅ Copy the csproj file first
COPY CommandsService/CommandsService.csproj CommandsService/

# ✅ Restore dependencies
RUN dotnet restore PlatformService/PlatformService.csproj

# ✅ Copy everything else
COPY . .

# ✅ Set working directory to the project folder
WORKDIR /src/CommandsService
RUN dotnet build CommandsService.csproj -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish CommandsService.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app


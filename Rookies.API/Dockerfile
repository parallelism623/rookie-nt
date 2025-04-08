# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rookies.API/Rookies.API.csproj", "Rookies.API/"]
COPY ["Rookies.Application/Rookies.Application.csproj", "Rookies.Application/"]
COPY ["Rookies.Contract/Rookies.Contract.csproj", "Rookies.Contract/"]
COPY ["Rookies.Domain/Rookies.Domain.csproj", "Rookies.Domain/"]
COPY ["Rookies.Infrastructure/Rookies.Infrastructure.csproj", "Rookies.Infrastructure/"]
COPY ["Rookies.Persistence/Rookies.Persistence.csproj", "Rookies.Persistence/"]
RUN dotnet restore "./Rookies.API/Rookies.API.csproj"
COPY . .
WORKDIR "/src/Rookies.API"
RUN dotnet build "./Rookies.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rookies.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rookies.API.dll"]
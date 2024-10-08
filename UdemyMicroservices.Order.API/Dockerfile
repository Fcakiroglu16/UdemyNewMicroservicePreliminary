# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UdemyMicroservices.Order.API/UdemyMicroservices.Order.API.csproj", "UdemyMicroservices.Order.API/"]
COPY ["UdemyMicroservices.Order.Application/UdemyMicroservices.Order.Application.csproj", "UdemyMicroservices.Order.Application/"]
COPY ["UdemyMicroservices.Bus/UdemyMicroservices.Bus.csproj", "UdemyMicroservices.Bus/"]
COPY ["UdemyMicroservices.Order.Domain/UdemyMicroservices.Order.Domain.csproj", "UdemyMicroservices.Order.Domain/"]
COPY ["UdemyMicroservives.Shared/UdemyMicroservices.Shared.csproj", "UdemyMicroservives.Shared/"]
COPY ["UdemyMicroservices.Order.Repository/UdemyMicroservices.Order.Persistence.csproj", "UdemyMicroservices.Order.Repository/"]
RUN dotnet restore "./UdemyMicroservices.Order.API/UdemyMicroservices.Order.API.csproj"
COPY . .
WORKDIR "/src/UdemyMicroservices.Order.API"
RUN dotnet build "./UdemyMicroservices.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UdemyMicroservices.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UdemyMicroservices.Order.API.dll"]
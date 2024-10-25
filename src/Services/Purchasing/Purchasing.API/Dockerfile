#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Purchasing/Purchasing.API/Purchasing.API.csproj", "Services/Purchasing/Purchasing.API/"]
COPY ["Services/Purchasing/Purchasing.Application/Purchasing.Application.csproj", "Services/Purchasing/Purchasing.Application/"]
COPY ["Common/Common.Messaging/Common.Messaging.csproj", "Common/Common.Messaging/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
COPY ["Services/Purchasing/Purchasing.Domain/Purchasing.Domain.csproj", "Services/Purchasing/Purchasing.Domain/"]
COPY ["Services/Purchasing/Purchasing.Infrastructure/Purchasing.Infrastructure.csproj", "Services/Purchasing/Purchasing.Infrastructure/"]
RUN dotnet restore "./Services/Purchasing/Purchasing.API/Purchasing.API.csproj"
COPY . .
WORKDIR "/src/Services/Purchasing/Purchasing.API"
RUN dotnet build "./Purchasing.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Purchasing.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Purchasing.API.dll"]
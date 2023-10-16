#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_URLS=http://*:8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DrivingSchoolPlatform.Api/DrivingSchoolPlatform.Api.csproj", "DrivingSchoolPlatform.Api/"]
RUN dotnet restore "DrivingSchoolPlatform.Api/DrivingSchoolPlatform.Api.csproj"
COPY . .
WORKDIR "/src/DrivingSchoolPlatform.Api"
RUN dotnet build "DrivingSchoolPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "DrivingSchoolPlatform.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DrivingSchoolPlatform.Api.dll"]
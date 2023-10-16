#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DrivingSchoolPlatform/DrivingSchoolPlatform/DrivingSchoolPlatform.csproj", "DrivingSchoolPlatform/DrivingSchoolPlatform/"]
COPY ["DrivingSchoolPlatform/DrivingSchoolPlatform.Client/DrivingSchoolPlatform.Client.csproj", "DrivingSchoolPlatform/DrivingSchoolPlatform.Client/"]
RUN dotnet restore "./DrivingSchoolPlatform/DrivingSchoolPlatform/./DrivingSchoolPlatform.csproj"
COPY . .
WORKDIR "/src/DrivingSchoolPlatform/DrivingSchoolPlatform"
RUN dotnet build "./DrivingSchoolPlatform.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DrivingSchoolPlatform.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DrivingSchoolPlatform.dll"]
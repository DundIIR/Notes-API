#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

# FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081

# FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["Notes-API/Notes-API.csproj", "Notes-API/"]
# RUN dotnet restore "./Notes-API/Notes-API.csproj"
# COPY . .
# WORKDIR "/src/Notes-API"
# RUN dotnet build "./Notes-API.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./Notes-API.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Notes-API.dll"]
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# COPY FirstChat/bin/Debug/net8.0/ /app
# ENTRYPOINT ["dotnet", "FirstChat.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
#EXPOSE 7116

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . .
#"FirstChat.csproj"
RUN dotnet build -c Release -o /app/build

#FROM build AS publish
#RUN dotnet build "FirstChat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/build .

ENTRYPOINT ["dotnet", "Notes-API.dll"]



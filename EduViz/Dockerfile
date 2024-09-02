# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["EduViz.csproj", "./"]
RUN dotnet restore "./EduViz.csproj"

# Copy the entire project directory and build the project
COPY . .
WORKDIR "/src"
RUN dotnet build "EduViz.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "EduViz.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image and copy the publish output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduViz.dll"]

# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ thư mục hiện tại (.) vào thư mục /src trong container
COPY . .

# Restore the dependencies
RUN dotnet restore "PostService.csproj"

# Build the application
RUN dotnet build "PostService.csproj" -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "PostService.csproj" -c Release -o /app/publish

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .


EXPOSE 5003

# Entry point to run the application
ENTRYPOINT ["dotnet", "PostService.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["4roomforum.csproj", "."]
RUN dotnet restore "4roomforum.csproj"

COPY . .
RUN dotnet publish "4roomforum.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5287
ENV ASPNETCORE_URLS=http://+:5287

ENTRYPOINT ["dotnet", "4roomforum.dll"]
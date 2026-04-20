# Stage 1: Build
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["prjHealthCareSystem.csproj", "./"]
RUN dotnet restore "./prjHealthCareSystem.csproj"
COPY . .
RUN dotnet publish "prjHealthCareSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Final
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "prjHealthCareSystem.dll"]
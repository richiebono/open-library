FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/LaunchQ.TakeHomeProject.sln", "./"]
COPY ["src/Domain/LaunchQ.TakeHomeProject.Domain.csproj", "Domain/"]
COPY ["src/Application/LaunchQ.TakeHomeProject.Application.csproj", "Application/"]
COPY ["src/Infrastructure/LaunchQ.TakeHomeProject.Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Presentation/Blazor/LaunchQ.TakeHomeProject.Presentation.Blazor.csproj", "Presentation/Blazor/"]
COPY ["src/Tests/UnitTests/LaunchQ.TakeHomeProject.UnitTests.csproj", "Tests/UnitTests/"]
COPY ["src/Tests/IntegrationTests/LaunchQ.TakeHomeProject.IntegrationTests.csproj", "Tests/IntegrationTests/"]

RUN dotnet restore "LaunchQ.TakeHomeProject.sln"

COPY ["src/Domain/", "Domain/"]
COPY ["src/Application/", "Application/"]
COPY ["src/Infrastructure/", "Infrastructure/"]
COPY ["src/Presentation/Blazor/", "Presentation/Blazor/"]
COPY ["src/Tests/UnitTests/", "Tests/UnitTests/"]
COPY ["src/Tests/IntegrationTests/", "Tests/IntegrationTests/"]

WORKDIR "/src"
# Build all projects including tests
RUN dotnet build "LaunchQ.TakeHomeProject.sln" -c Release -o /app/build

WORKDIR "/src/Presentation/Blazor"

FROM build AS publish
RUN dotnet publish "LaunchQ.TakeHomeProject.Presentation.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "LaunchQ.TakeHomeProject.Presentation.Blazor.dll"]

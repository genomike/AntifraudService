FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/AntifraudService.Worker/AntifraudService.Worker.csproj", "src/AntifraudService.Worker/"]
COPY ["src/AntifraudService.Application/AntifraudService.Application.csproj", "src/AntifraudService.Application/"]
COPY ["src/AntifraudService.Infrastructure/AntifraudService.Infrastructure.csproj", "src/AntifraudService.Infrastructure/"]
COPY ["src/AntifraudService.Domain/AntifraudService.Domain.csproj", "src/AntifraudService.Domain/"]
RUN dotnet restore "src/AntifraudService.Worker/AntifraudService.Worker.csproj"
COPY . .
WORKDIR "/src/src/AntifraudService.Worker"
RUN dotnet build "AntifraudService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AntifraudService.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntifraudService.Worker.dll"]
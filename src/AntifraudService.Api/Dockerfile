FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/AntifraudService.Api/AntifraudService.Api.csproj", "src/AntifraudService.Api/"]
COPY ["src/AntifraudService.Application/AntifraudService.Application.csproj", "src/AntifraudService.Application/"]
COPY ["src/AntifraudService.Infrastructure/AntifraudService.Infrastructure.csproj", "src/AntifraudService.Infrastructure/"]
COPY ["src/AntifraudService.Domain/AntifraudService.Domain.csproj", "src/AntifraudService.Domain/"]
RUN dotnet restore "src/AntifraudService.Api/AntifraudService.Api.csproj"
COPY . .
WORKDIR "/src/src/AntifraudService.Api"
RUN dotnet build "AntifraudService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AntifraudService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AntifraudService.Api.dll"]
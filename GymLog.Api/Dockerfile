FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GymLog.Api/GymLog.Api.csproj", "GymLog.Api/"]
COPY ["GymLog.Infrastructure/GymLog.Infrastructure.csproj", "GymLog.Infrastructure/"]
COPY ["GymLog.Application/GymLog.Application.csproj", "GymLog.Application/"]
COPY ["GymLog.Domain/GymLog.Domain.csproj", "GymLog.Domain/"]
RUN dotnet restore "GymLog.Api/GymLog.Api.csproj"
COPY . .
WORKDIR "/src/GymLog.Api"
RUN dotnet build "GymLog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GymLog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GymLog.Api.dll"]

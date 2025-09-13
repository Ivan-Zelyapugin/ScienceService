# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Science.WebApi/Science.WebApi.csproj", "Science.WebApi/"]
COPY ["Science.Persistence/Science.Persistence.csproj", "Science.Persistence/"]
COPY ["Science.Application/Science.Application.csproj", "Science.Application/"]
COPY ["Science.Domain/Science.Domain.csproj", "Science.Domain/"]
RUN dotnet restore "./Science.WebApi/Science.WebApi.csproj"
COPY . .
WORKDIR "/src/Science.WebApi"
RUN dotnet build "./Science.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Science.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Science.WebApi.dll"]
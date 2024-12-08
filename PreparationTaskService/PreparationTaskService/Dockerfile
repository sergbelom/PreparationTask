FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PreparationTaskService/PreparationTaskService.csproj", "PreparationTaskService/"]
RUN dotnet restore "PreparationTaskService/PreparationTaskService.csproj"
COPY . .
WORKDIR "/src/PreparationTaskService"
RUN dotnet build "PreparationTaskService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PreparationTaskService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PreparationTaskService.dll"]

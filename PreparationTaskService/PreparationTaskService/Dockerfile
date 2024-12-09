FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PreparationTaskService/PreparationTaskService.csproj", "PreparationTaskService/"]
COPY ["PreparationTaskService.DAL/PreparationTaskService.DAL.csproj", "PreparationTaskService.DAL/"]
COPY ["PreparationTaskService.DAL.Entities/PreparationTaskService.DAL.Entities.csproj", "PreparationTaskService.DAL.Entities/"]
COPY ["PreparationTaskService.DataTransfer/PreparationTaskService.DataTransfer.csproj", "PreparationTaskService.DataTransfer/"]
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
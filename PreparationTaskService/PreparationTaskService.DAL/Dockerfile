# Dockerfile for migration service
FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app
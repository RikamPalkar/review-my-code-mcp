# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY McpCodeReviewServer.csproj ./
RUN dotnet restore McpCodeReviewServer.csproj

COPY . .
RUN dotnet publish McpCodeReviewServer.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
COPY --from=build /src/documentation ./documentation

ENTRYPOINT ["dotnet", "McpCodeReviewServer.dll"]

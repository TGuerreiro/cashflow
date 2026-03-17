# Runtime leve (Alpine) para produção
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# SDK completo para o build (usando Alpine para consistencia)
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["src/CashFlow.Shared/CashFlow.Shared.Domain/CashFlow.Shared.Domain.csproj", "src/CashFlow.Shared/CashFlow.Shared.Domain/"]
COPY ["src/CashFlow.Shared/CashFlow.Shared.Messaging/CashFlow.Shared.Messaging.csproj", "src/CashFlow.Shared/CashFlow.Shared.Messaging/"]
COPY ["src/CashFlow.Consolidado/CashFlow.Consolidado.Domain/CashFlow.Consolidado.Domain.csproj", "src/CashFlow.Consolidado/CashFlow.Consolidado.Domain/"]
COPY ["src/CashFlow.Consolidado/CashFlow.Consolidado.Application/CashFlow.Consolidado.Application.csproj", "src/CashFlow.Consolidado/CashFlow.Consolidado.Application/"]
COPY ["src/CashFlow.Consolidado/CashFlow.Consolidado.Infrastructure/CashFlow.Consolidado.Infrastructure.csproj", "src/CashFlow.Consolidado/CashFlow.Consolidado.Infrastructure/"]
COPY ["src/CashFlow.Consolidado/CashFlow.Consolidado.API/CashFlow.Consolidado.API.csproj", "src/CashFlow.Consolidado/CashFlow.Consolidado.API/"]
COPY ["Directory.Build.props", "."]

RUN dotnet restore "src/CashFlow.Consolidado/CashFlow.Consolidado.API/CashFlow.Consolidado.API.csproj"

COPY src/CashFlow.Shared/ src/CashFlow.Shared/
COPY src/CashFlow.Consolidado/ src/CashFlow.Consolidado/

RUN dotnet publish "src/CashFlow.Consolidado/CashFlow.Consolidado.API/CashFlow.Consolidado.API.csproj" \
    -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CashFlow.Consolidado.API.dll"]

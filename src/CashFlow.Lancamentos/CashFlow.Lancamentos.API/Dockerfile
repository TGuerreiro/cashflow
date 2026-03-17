# Runtime leve (Alpine) para "produção"
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["src/CashFlow.Shared/CashFlow.Shared.Domain/CashFlow.Shared.Domain.csproj", "src/CashFlow.Shared/CashFlow.Shared.Domain/"]
COPY ["src/CashFlow.Shared/CashFlow.Shared.Messaging/CashFlow.Shared.Messaging.csproj", "src/CashFlow.Shared/CashFlow.Shared.Messaging/"]
COPY ["src/CashFlow.Lancamentos/CashFlow.Lancamentos.Domain/CashFlow.Lancamentos.Domain.csproj", "src/CashFlow.Lancamentos/CashFlow.Lancamentos.Domain/"]
COPY ["src/CashFlow.Lancamentos/CashFlow.Lancamentos.Application/CashFlow.Lancamentos.Application.csproj", "src/CashFlow.Lancamentos/CashFlow.Lancamentos.Application/"]
COPY ["src/CashFlow.Lancamentos/CashFlow.Lancamentos.Infrastructure/CashFlow.Lancamentos.Infrastructure.csproj", "src/CashFlow.Lancamentos/CashFlow.Lancamentos.Infrastructure/"]
COPY ["src/CashFlow.Lancamentos/CashFlow.Lancamentos.API/CashFlow.Lancamentos.API.csproj", "src/CashFlow.Lancamentos/CashFlow.Lancamentos.API/"]
COPY ["Directory.Build.props", "."]

RUN dotnet restore "src/CashFlow.Lancamentos/CashFlow.Lancamentos.API/CashFlow.Lancamentos.API.csproj"

COPY src/CashFlow.Shared/ src/CashFlow.Shared/
COPY src/CashFlow.Lancamentos/ src/CashFlow.Lancamentos/

RUN dotnet publish "src/CashFlow.Lancamentos/CashFlow.Lancamentos.API/CashFlow.Lancamentos.API.csproj" \
    -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CashFlow.Lancamentos.API.dll"]

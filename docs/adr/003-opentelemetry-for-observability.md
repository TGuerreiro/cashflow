# ADR 003: Observabilidade com OpenTelemetry

## Status
Aceito

## Contexto
Em uma arquitetura de microsserviços, o rastreamento de requisições que cruzam as fronteiras dos serviços é fundamental para diagnóstico e monitoramento de performance.

## Decisão
Implementar o **OpenTelemetry** para rastreamento distribuído (Tracing) e métricas.
1. Padronização via W3C Trace Context.
2. Integração com MassTransit para visualizar o fluxo das mensagens entre os serviços.
3. Coleta de métricas de performance do runtime do .NET.

## Consequências
- **Positivas**: Visibilidade ponta a ponta das transações, facilidade na identificação de gargalos e erros.
- **Negativas**: Pequeno overhead de processamento para geração de telemetria.

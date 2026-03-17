# ADR 002: PostgreSQL como Banco de Dados Principal

## Status
Aceito

## Contexto
O sistema precisa de uma persistência robusta, suporte a transações ACID para o Outbox Pattern e alta performance para consultas financeiras.

## Decisão
Utilizar o **PostgreSQL** para ambos os microsserviços (Lançamentos e Consolidado).
1. Suporte nativo a transações robustas.
2. Integração excelente com o EF Core e MassTransit.
3. Flexibilidade para crescimento futuro (JSONB e particionamento).

## Consequências
- **Positivas**: Confiabilidade dos dados, ecossistema maduro, suporte a transações complexas.
- **Negativas**: Requer gerenciamento de infraestrutura (containers/instâncias).

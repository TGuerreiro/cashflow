# ADR 001: Implementação de Outbox Pattern com MassTransit

## Status
Aceito

## Contexto
O sistema precisa garantir a consistência entre o banco de dados de lançamentos e o processamento assíncrono no serviço consolidado. Uma falha ao publicar uma mensagem no RabbitMQ após salvar no banco resultaria em dados divergentes.

## Decisão
Implementamos o **Outbox Pattern** utilizando as capacidades nativas do **MassTransit**.
1. As mensagens são salvas na mesma transação do banco de dados PostgreSQL.
2. Um worker em background do MassTransit processa essas mensagens e as envia para o RabbitMQ.
3. No lado do consumidor (Consolidado), utilizamos o **Inbox Pattern** para garantir a idempotência do processamento.

## Consequências
- **Positivas**: Garantia de consistência eventual ("At-least-once delivery"), resiliência a falhas temporárias do broker de mensagens.
- **Negativas**: Pequeno aumento na latência de persistência inicial e complexidade adicional no esquema do banco de dados.

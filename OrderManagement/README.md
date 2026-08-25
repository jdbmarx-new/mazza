# Order Management API (.NET 10)

Implementação do teste prático com DDD tático, Clean Architecture, CQRS/MediatR, EF Core/SQLite, JWT, FluentValidation, Serilog, OpenTelemetry, xUnit e Docker.

## Decisões

- **Controllers**: foram escolhidos porque as cinco operações HTTP formam uma API de recursos convencional. Atributos de rota, autorização e metadados ficam explícitos, enquanto os controllers permanecem simples, sem regras de negócio e delegam os casos de uso ao MediatR.
- **DDD**: `Order` é a raiz do agregado e controla seus itens, estado e cálculo de `TotalAmount`. Não há setters públicos nem lógica de negócio em API ou Infrastructure.
- **Repositório**: `IOrderRepository` expressa apenas as necessidades do agregado. Um repositório genérico adicionaria abstração sem valor.
- **CQRS simples**: commands e queries são separados, sem criar dois bancos ou introduzir complexidade desnecessária.

## Executar localmente

Requer SDK .NET 10.

```bash
dotnet restore OrderManagement.slnx
dotnet test OrderManagement.slnx
dotnet run --project src/OrderManagement.Api
```

As migrations são aplicadas automaticamente no startup. A API usa `orders.db` por padrão.

## Docker

```bash
docker compose up --build
```

API: `http://localhost:8080`. SonarQube opcional: `docker compose --profile quality up sonarqube`.

## Login

`POST /auth/login`

```json
{"email":"dev@mazza.tech","password":"Senha@123"}
```

Use `Authorization: Bearer <token>` nos endpoints `/api/orders`.

## Exemplo de criação

```json
{"customerId":"11111111-1111-1111-1111-111111111111","items":[{"productName":"Keyboard","quantity":2,"unitPrice":150.00}]}
```

## Segurança

A chave no `appsettings.json` é apenas para desenvolvimento e deve ser substituída por variável de ambiente ou secret store. O login fixo foi mantido por exigência do exercício. Logs do pipeline podem conter dados de commands/queries, portanto devem receber redaction em produção.

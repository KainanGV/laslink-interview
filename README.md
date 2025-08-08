# LastLink Anticipation API

API simples para **gestão de solicitações de antecipação de valores** (Clean Architecture friendly), com regras de negócio, testes automatizados e documentação via Swagger.

## ⚙️ Requisitos
- .NET SDK **9.0+**
- (opcional) Postman ou Insomnia para testar os endpoints

## ▶️ Como executar
```bash
# Restaurar dependências
dotnet restore

# Rodar os testes
dotnet test

# Subir a API (swagger on)
dotnet run --project src/LastLink.Anticipation.Api
```
Você verá no console algo como:
```
Now listening on: https://localhost:7032
Now listening on: http://localhost:5036
```

### Swagger / OpenAPI
- **HTTPS**: https://localhost:7032/swagger
- **HTTP**:  http://localhost:5036/swagger

> Em `src/LastLink.Anticipation.Api/Program.cs`, o Swagger está habilitado por padrão no ambiente *Development*. Também há um *middleware* de exceções que mapeia erros para **ProblemDetails**.

## 🧪 Testes
Os testes vivem em `tests/LastLink.Anticipation.Tests`. Para rodar:

```bash
dotnet test
```

## 📚 Regras de Negócio
- Valor solicitado **>= R$ 100,00**
- Um creator **não pode ter** mais de uma solicitação **pendente** ao mesmo tempo
- Taxa de antecipação fixa: **5%**
- Solicitações começam com status **"pendente"**

## 🛣️ Endpoints (V1)

> Base path padrão: `/api/AnticipationRequests`

### Criar solicitação
- **POST** `/api/AnticipationRequests`
- Body (JSON):
```json
{
  "creatorId": "GUID AQUI",
  "requestedAmount": 250.00,
  "requestedAt": "2025-01-01T12:00:00Z"
}
```
- **201 Created** → retorna a entidade criada
- **400** (validação), **409** (conflito)

### Listar por creator
- **GET** `/api/AnticipationRequests/{creatorId}`
- **200 OK** → lista

### Aprovar
- **PUT** `/api/AnticipationRequests/{id}/approve`
- **204 No Content** | **404 Not Found**

### Rejeitar
- **PUT** `/api/AnticipationRequests/{id}/reject`
- **204 No Content** | **404 Not Found**

### Simular (sem criar)
- **GET** `/api/AnticipationRequests/simulate?requestedAmount=1000`
- **200 OK**:
```json
{
  "requestedAmount": 1000.00,
  "feePercentage": 0.05,
  "feeAmount": 50.00,
  "netAmount": 950.00
}
```

## 🧩 Erros (ProblemDetails)
As principais exceções de aplicação são mapeadas para ProblemDetails:
- `AppValidationException` → **400 Bad Request**
- `ConflictException` → **409 Conflict**
- `NotFoundException` → **404 Not Found**

Exemplo de resposta 400:
```json
{
  "type": "about:blank",
  "title": "Validation error",
  "status": 400,
  "detail": "Requested amount must be at least R$ 100,00.",
  "errors": {
    "requestedAmount": ["min 100"]
  }
}
```

## 🧱 Arquitetura (visão rápida)
- **Domain**: entidades e contratos de repositório
- **Application**: use cases, serviços e exceções de aplicação
- **Infra**: EF Core InMemory + repositórios concretos
- **Api**: Controllers, DI e Middleware (ProblemDetails)

## 🚀 Postman
Importe a collection `postman/LastLink-Anticipation.postman_collection.json` e selecione o ambiente local (variável `baseUrl`).

---

> Última atualização: 2025-08-08

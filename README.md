# Short URL Web API (minimal)

Projeto .NET minimal API para encurtar URLs (in-memory).

Requisitos
- .NET 10 SDK

Executar

Windows / PowerShell:

```powershell
dotnet restore
dotnet run --project .
```

Exemplos

1) Criar um short URL (POST JSON):

```bash
curl -X POST http://localhost:5000/shorten -H "Content-Type: application/json" -d '{"url":"https://example.com/very/long/path"}'
```

Resposta esperada:

```json
{ "key": "abc123", "shortUrl": "http://localhost:5000/abc123" }
```

2) Acessar short URL (GET):

Abra `http://localhost:5000/{key}` no navegador — a API fará redirect para a URL original.

Observações
- O armazenamento é in-memory; reiniciar a aplicação perde os mapeamentos.
- Para produção, substitua por um armazenamento persistente (DB, Redis, etc.) e adicione validação/limitação.

Bitly Integration
- To use Bitly instead of the local in-memory shortener, set the environment variable `BITLY_TOKEN` with your Bitly Generic Access Token.

Example (PowerShell):

```powershell
$env:BITLY_TOKEN = "YOUR_BITLY_TOKEN"
dotnet run --project .
```

Shorten via Bitly (POST):

```bash
curl -X POST http://localhost:5000/shorten/bitly -H "Content-Type: application/json" -d '{"url":"https://example.com/very/long/path"}'
```

Response example:

```json
{ "link": "https://bit.ly/xyz123" }
```

If `BITLY_TOKEN` is not set, the `/shorten/bitly` endpoint will return a 400 with an explanatory message.

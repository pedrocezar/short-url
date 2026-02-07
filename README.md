# Short URL Web API (minimal)

Minimal .NET API project to shorten URLs (in-memory).

Requirements
- .NET 10 SDK

Run

Windows / PowerShell:

```powershell
dotnet restore
dotnet run --project .
```

Examples

1) Create a short URL (POST JSON):

```bash
curl -X POST http://localhost:5000/shorten -H "Content-Type: application/json" -d '{"url":"https://example.com/very/long/path"}'
```

Expected response:

```json
{ "key": "abc123", "shortUrl": "http://localhost:5000/abc123" }
```

2) Access short URL (GET):

Open `http://localhost:5000/{key}` in your browser — the API will redirect to the original URL.

Notes
- Storage is in-memory; restarting the application will lose mappings.
- For production, replace with a persistent store (DB, Redis, etc.) and add rate-limiting/validation.

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

# Short URL Web API (minimal)

Minimal .NET API to shorten URLs with in-memory storage.

**Prerequisites**
- .NET 10 SDK (see `TargetFramework` in `ShortUrl.Api.csproj`).

**Run**

Windows / PowerShell:

```powershell
dotnet restore
dotnet run --project .
```

The application will start and show the listening addresses (for example `http://localhost:5000`).

**Endpoints**
- `POST /shorten` — create a short link. Accepts JSON with the `url` property.
  - Example request:

```bash
curl -X POST http://localhost:5000/shorten \
  -H "Content-Type: application/json" \
  -d '{"url":"https://example.com/very/long/path"}'
```

  - Example response (200):

```json
{ "key": "abc123", "shortUrl": "http://localhost:5000/abc123" }
```

- `GET /{key}` — redirects to the original URL (302 if found, 404 if not).
- `GET /url/{key}` — returns the original URL as JSON (200 `{ "url": "..." }` or 404).

**Swagger / OpenAPI**
- Swagger UI is available at `/swagger` while the app is running.

**Limitations & recommendations**
- Storage is in-memory — restarting the app will lose all mappings.
- For production: use a persistent store (database or Redis), add authentication, rate limiting and monitoring.

**Technical notes**
- Input validation uses `FluentValidation` (see `Validators/ShortenRequestValidator.cs`).
- Target framework: `net10.0` (see `ShortUrl.Api.csproj`).

Contributions and improvements are welcome — please open an issue or pull request.
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

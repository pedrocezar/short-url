# Short URL Web API

Minimal .NET API to shorten URLs with in-memory storage. Built with .NET 10 minimal APIs, FluentValidation, and OpenAPI/Scalar documentation.

## Table of contents

- [Prerequisites](#prerequisites)
- [Getting started](#getting-started)
- [API endpoints](#api-endpoints)
- [Examples](#examples)
- [API documentation](#api-documentation)
- [Project structure](#project-structure)
- [Limitations & recommendations](#limitations--recommendations)

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (see `TargetFramework` in `ShortUrl.Api.csproj`).

## Getting started

**Restore and run** (Windows / PowerShell):

```powershell
dotnet restore
dotnet run --project .
```

The app will start and display the listening addresses (e.g. `http://localhost:5000`).

**Root path:** `GET /` redirects to the API documentation at `/scalar`.

## API endpoints

| Method | Path        | Description |
|--------|-------------|-------------|
| `GET`  | `/`         | Redirects to `/scalar` (API docs). |
| `POST` | `/shorten`  | Creates a short link. Body: `{ "url": "https://..." }`. |
| `GET`  | `/{key}`    | Redirects (302) to the original URL, or 404 if not found. |
| `GET`  | `/url/{key}`| Returns the original URL as JSON: `{ "url": "..." }`, or 404. |

- **Validation:** `POST /shorten` accepts only valid HTTP/HTTPS URLs. Invalid input returns `400` with validation errors (FluentValidation).

## Examples

**Create a short URL:**

```bash
curl -X POST http://localhost:5000/shorten \
  -H "Content-Type: application/json" \
  -d "{\"url\":\"https://example.com/very/long/path\"}"
```

**Response (200):**

```json
{ "key": "abc123", "shortUrl": "http://localhost:5000/abc123" }
```

**Redirect to original URL:**  
Open `http://localhost:5000/{key}` in a browser, or:

```bash
curl -L http://localhost:5000/abc123
```

**Get original URL as JSON:**

```bash
curl http://localhost:5000/url/abc123
```

**Response (200):**

```json
{ "url": "https://example.com/very/long/path" }
```

## API documentation

- **Scalar:** `http://localhost:5000/scalar` — interactive API reference while the app is running.
- OpenAPI spec is exposed for tooling (e.g. `MapOpenApi()`).

## Project structure

```
ShortUrl/
├── Program.cs                 # Minimal API setup, endpoints, Scalar
├── ShortUrl.Api.csproj        # .NET 10, FluentValidation, Scalar, OpenAPI
├── Models/
│   └── ShortenRequest.cs      # Request model for POST /shorten
├── Services/
│   └── ShortUrlService.cs     # In-memory shorten/resolve (ConcurrentDictionary)
└── Validators/
    └── ShortenRequestValidator.cs  # URL validation (required, valid HTTP/HTTPS)
```

## Limitations & recommendations

- **Storage:** In-memory only. Restarting the application clears all short URLs.
- **Production:** Use a persistent store (database or Redis), add authentication, rate limiting, and monitoring.

---

Contributions and improvements are welcome — open an issue or pull request.

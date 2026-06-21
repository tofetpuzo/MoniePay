<div align="center">

# MoniePay

**A modern payments platform built on ASP.NET Core 9, Blazor United, and PostgreSQL.**

Payment intents, ledger-style double-entry accounting, idempotent attempts, and webhook delivery — modeled the way real payment processors do it.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-United-512BD4?logo=blazor&logoColor=white)](https://learn.microsoft.com/aspnet/core/blazor/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4)](https://learn.microsoft.com/ef/core/)
[![JWT](https://img.shields.io/badge/Auth-JWT%20%2B%20Identity-000000?logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![License](https://img.shields.io/badge/License-See%20LICENSE-blue)](LICENSE)

[Architecture](docs/ARCHITECTURE.md) · [Quick Start](#quick-start) · [API](#api) · [Roadmap](#roadmap)

</div>

---

## Overview

MoniePay is a full-stack payments service that models the lifecycle of a charge the way mature processors (Stripe, Paystack, Adyen) do — separating **intent**, **attempt**, **settlement**, and **ledger entry** as first-class concepts. The codebase is deliberately small enough to read in one sitting, but follows production-grade patterns so the design scales beyond the demo.

The application is a single ASP.NET Core 9 host that exposes:

- A **REST API** under `/api/*` for headless integration (mobile clients, partner servers).
- An **interactive web UI** built with Blazor United (Server + WebAssembly auto render modes).
- **OpenAPI/Swagger** docs at `/swagger` in development.

## Highlights

| Capability | What it means |
|---|---|
| **Payment intent lifecycle** | `PaymentIntent → PaymentAttempt → Payment → Transaction → Payout` is modeled as separate aggregates so retries, refunds, and reconciliation are clean. |
| **Double-entry ledger** | Every money movement creates paired `LedgerEntries` against `LedgerAccounts`, making balances auditable and reversible. |
| **Idempotency built in** | `PaymentIntents.IdempotencyKey` prevents duplicate charges when clients retry. |
| **Event sourcing surface** | `EventStore` table captures domain events for replay, audit, and downstream projections. |
| **Webhooks with delivery tracking** | `Webhooks` + `WebhookDeliveries` record subscribers, attempts, and outcomes — not fire-and-forget. |
| **Identity + JWT** | ASP.NET Identity (`IdentityUser<Guid>`) for storage, JWT bearer tokens for stateless API auth. |
| **Flag-based RBAC** | `[Flags] enum RoleType` lets a single user combine `Admin \| Finance \| Support` without join-table proliferation. |
| **Friendly DB schema** | Identity tables renamed (`Users`, `UserRoles`, …) so the schema reads like a real product, not a framework dump. |

## Tech Stack

**Backend**
- ASP.NET Core 9 (Minimal hosting, Controllers, Razor Components)
- Entity Framework Core 9 + Npgsql (PostgreSQL provider)
- ASP.NET Identity Core (custom `User : IdentityUser<Guid>`)
- JWT Bearer authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Swashbuckle / OpenAPI for API docs

**Frontend**
- Blazor United — Server interactive + WebAssembly interactive auto modes
- Shared `MoniePay.Client` project for component reuse

**Data**
- PostgreSQL 16
- EF Core migrations (code-first)
- Snake-friendly table mapping for Identity entities

## Architecture at a Glance

```
                ┌─────────────────────────────────────────────┐
                │                Browser / Client             │
                │   (Blazor UI · Mobile App · Partner API)    │
                └──────────────┬──────────────────────────────┘
                               │  HTTPS · JWT
                               ▼
        ┌────────────────────────────────────────────────────────┐
        │                ASP.NET Core 9 Host                     │
        │  ┌──────────────┐  ┌───────────────┐  ┌─────────────┐  │
        │  │  Controllers │  │   Services    │  │   Blazor    │  │
        │  │  (REST API)  │◄─┤ Identity, …   │─►│ Components  │  │
        │  └──────┬───────┘  └───────┬───────┘  └─────────────┘  │
        │         │                  │                           │
        │         ▼                  ▼                           │
        │  ┌──────────────────────────────────────────────────┐  │
        │  │              AppDbContext (EF Core)              │  │
        │  └───────────────────────┬──────────────────────────┘  │
        └──────────────────────────┼─────────────────────────────┘
                                   ▼
                          ┌──────────────────┐
                          │   PostgreSQL     │
                          │  Users · Intents │
                          │  Ledger · Events │
                          └──────────────────┘
```

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for the full component diagram, ER model, and request-flow sequence diagrams.

## Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/) running locally (or a connection string to a hosted instance)
- `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`

### 1. Clone & configure

```bash
git clone <your-fork-url> MoniePay
cd MoniePay
```

Edit `MoniePay/appsettings.Development.json` and set your PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=moniepay;Username=postgres;Password=postgres"
  },
  "JwtSettings": {
    "SecretKey": "replace-with-a-long-random-secret",
    "Issuer": "MoniePay",
    "Audience": "MoniePayClients"
  }
}
```

### 2. Apply migrations

```bash
cd MoniePay
dotnet ef database update
```

### 3. Run

```bash
dotnet run
```

The app starts on `https://localhost:7xxx`. Swagger UI is available at `/swagger`.

## API

Auth endpoints live under `/api/auth`:

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/auth/create` | Register a new user. Body: `{ "username", "password", "email" }` |

Example:

```bash
curl -X POST https://localhost:7xxx/api/auth/create \
  -H "Content-Type: application/json" \
  -d '{"username":"jane","password":"P@ssw0rd!","email":"jane@example.com"}'
```

Full OpenAPI spec: `https://localhost:7xxx/swagger`.

## Project Structure

```
MoniePay/
├── MoniePay/                       # Server host (API + Blazor Server)
│   ├── Program.cs                  # Composition root
│   ├── Components/                 # Razor components (Pages, Layout)
│   ├── Migrations/                 # EF Core migrations
│   └── src/
│       ├── auth/                   # User, Roles (RBAC flags)
│       ├── controller/             # API controllers
│       ├── data/                   # AppDbContext (IdentityDbContext)
│       ├── models/                 # Domain entities (PaymentIntent, Ledger, …)
│       └── services/               # IdentityService, DTOs
├── MoniePay.Client/                # Blazor WebAssembly client project
├── docs/                           # Architecture & design docs
└── MoniePay.sln
```

## Roadmap

- [x] Identity + JWT registration flow
- [x] Friendly Identity table names
- [x] Payment intent / attempt / payment / ledger model
- [ ] Login endpoint with refresh tokens
- [ ] Webhook delivery worker (background service)
- [ ] Provider adapter pattern (Stripe-style abstraction)
- [ ] Reconciliation report endpoint
- [ ] Docker Compose for one-command spin-up
- [ ] CI pipeline (GitHub Actions: build + test + EF migration check)

## License

See [LICENSE](LICENSE).

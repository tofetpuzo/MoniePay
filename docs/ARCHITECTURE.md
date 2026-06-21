# MoniePay — Architecture

This document describes how MoniePay is put together: the runtime topology, the domain model, request flow, and the design decisions behind each piece. It is intentionally aimed at engineers evaluating the codebase — every diagram is rendered by GitHub natively.

---

## 1. Runtime Topology

MoniePay is a **single ASP.NET Core 9 process** that hosts three surfaces from the same host:

1. A REST API (controllers under `/api/*`)
2. A Blazor United UI (Server + WebAssembly auto render modes)
3. OpenAPI / Swagger documentation

```mermaid
flowchart LR
    subgraph Clients
        B[Browser - Blazor UI]
        M[Mobile App / Partner Server]
        P[3rd-party Webhooks Consumer]
    end

    subgraph Host["ASP.NET Core 9 Host (MoniePay)"]
        direction TB
        MW[Middleware<br/>HTTPS · Auth · Antiforgery]
        API[REST Controllers<br/>/api/*]
        UI[Razor Components<br/>Server + WASM]
        SVC[Application Services<br/>IdentityService · …]
        DBCTX[(AppDbContext<br/>IdentityDbContext)]
        MW --> API
        MW --> UI
        API --> SVC
        UI --> SVC
        SVC --> DBCTX
    end

    DB[(PostgreSQL 16)]
    DBCTX --> DB

    B  -->|HTTPS| MW
    M  -->|HTTPS + JWT| MW
    Host -. outbound webhooks .-> P

    classDef ext fill:#1f2937,stroke:#9ca3af,color:#f9fafb
    classDef host fill:#0f172a,stroke:#38bdf8,color:#f9fafb
    classDef db fill:#1e3a8a,stroke:#60a5fa,color:#f9fafb
    class B,M,P ext
    class MW,API,UI,SVC,DBCTX host
    class DB db
```

**Why a single host?** The MoniePay scope is small enough that splitting API and UI into separate deployables would add ceremony without benefit. The seams are already drawn at the *service* layer, so extracting later is a refactor — not a rewrite.

---

## 2. Layered Responsibilities

```mermaid
flowchart TB
    subgraph Presentation
        C1[Controllers<br/>AuthController]
        C2[Razor Components<br/>Pages/Layout]
    end
    subgraph Application
        S1[IdentityService]
        S2[DTOs<br/>RegisterUser]
    end
    subgraph Domain
        D1[Aggregates<br/>PaymentIntent · Customer · Ledger]
        D2[Auth<br/>User · Roles]
    end
    subgraph Infrastructure
        I1[AppDbContext]
        I2[EF Core Migrations]
        I3[ASP.NET Identity Stores]
    end

    C1 --> S1
    C2 --> S1
    S1 --> D2
    S1 --> I1
    D1 --> I1
    D2 --> I3
    I1 --> I2
```

| Layer | Folder | Responsibility |
|---|---|---|
| **Presentation** | `src/controller/`, `Components/` | HTTP routing, render modes, model binding, validation. **Never** touches `DbContext` directly. |
| **Application** | `src/services/` | Use-case orchestration. Wraps `UserManager`, builds JWTs, enforces invariants. |
| **Domain** | `src/models/`, `src/auth/` | Pure entities. Constructors / value equality / business invariants live here. |
| **Infrastructure** | `src/data/`, `Migrations/` | `AppDbContext`, Identity store mapping, schema migration history. |

---

## 3. Domain Model

The domain is modeled after how real payment processors decompose a charge. **A single user-initiated purchase produces multiple persistent records**, each capturing a different concern.

```mermaid
erDiagram
    USER ||--o{ CUSTOMER : "owns profile"
    CUSTOMER ||--o{ PAYMENT_METHOD : "saves"
    CUSTOMER ||--o{ PAYMENT_INTENT : "initiates"
    PAYMENT_INTENT ||--o{ PAYMENT_ATTEMPT : "tries"
    PAYMENT_ATTEMPT ||--|| PAYMENT : "becomes (on success)"
    PAYMENT ||--|| TRANSACTION : "records"
    TRANSACTION ||--o{ LEDGER_ENTRY : "double-entry"
    LEDGER_ENTRY }o--|| LEDGER_ACCOUNT : "posts to"
    PAYMENT_INTENT ||--o| PAYOUT : "settles into"
    WEBHOOK ||--o{ WEBHOOK_DELIVERY : "fires"
    EVENT_STORE }o--|| PAYMENT_INTENT : "captures events for"

    USER {
        guid Id PK
        string UserName
        string Email
        string PasswordHash
        flags RoleFlags
        bool isActive
        datetime createdOn
    }
    CUSTOMER {
        guid Id PK
        string Email
        string FirstName
        string LastName
        string KycLevel
        bool IsVerifed
        bool IsBlackListed
        guid TenantId
    }
    PAYMENT_INTENT {
        guid Id PK
        guid CustomerId FK
        decimal Amount
        string Currency
        string Status
        string Channel
        string IdempotencyKey
        string Reference
    }
    PAYMENT_ATTEMPT {
        guid Id PK
        guid PaymentIntentId FK
        int AttemptNumber
        string Outcome
    }
    PAYMENT {
        guid Id PK
        guid PaymentIntentId FK
        string Provider
        string ProviderReference
        string Status
        bool IsFinal
    }
    LEDGER_ENTRY {
        guid Id PK
        guid AccountId FK
        decimal Debit
        decimal Credit
    }
```

### Why this shape?

- **Intent ≠ Charge.** A `PaymentIntent` is the *promise* to move money; the `Payment` is the realized event with the provider. Refunds, retries, and partial captures plug in cleanly because the intent persists across attempts.
- **Idempotency at the intent.** `IdempotencyKey` lives on `PaymentIntent` so a flaky client retrying `POST /intents` gets the same intent, not a duplicate charge.
- **Double-entry ledger.** Every state change posts paired debit/credit `LedgerEntries` against `LedgerAccounts`. Balances are derived, never stored — so they cannot drift.
- **EventStore as audit log.** A separate append-only stream means we can replay history, build read models, or hand a regulator a complete trail without joining 12 tables.

---

## 4. Authentication & Authorization

```mermaid
sequenceDiagram
    autonumber
    participant C as Client
    participant API as AuthController
    participant SVC as IdentityService
    participant UM as UserManager<User>
    participant DB as PostgreSQL

    Note over C,API: Registration
    C->>API: POST /api/auth/create<br/>{username, password, email}
    API->>SVC: RegisterUserAsync(dto)
    SVC->>UM: CreateAsync(user, password)
    UM->>DB: INSERT Users (hashed PW)
    UM-->>SVC: IdentityResult
    SVC-->>API: result
    API-->>C: 200 OK

    Note over C,API: Login (planned)
    C->>API: POST /api/auth/login
    API->>SVC: LoginAsync(creds)
    SVC->>UM: CheckPasswordAsync
    UM->>DB: SELECT Users
    SVC->>SVC: BuildUserClaims + sign JWT
    SVC-->>API: AccessTokenResponse
    API-->>C: { accessToken, refreshToken, expiresIn }
```

### Identity model

- `User : IdentityUser<Guid>` — uses `Guid` keys for distributed-friendly IDs.
- ASP.NET Identity tables are **renamed** in `AppDbContext.OnModelCreating`:
  - `AspNetUsers` → `Users`
  - `AspNetUserRoles` → `UserRoles`
  - `AspNetRoleClaims` → `RoleClaims` (etc.)
- The `User` class exposes lowercase JSON aliases (`username`, `email`, `userId`) marked `[NotMapped]` so the API contract is idiomatic JSON while the DB columns stay canonical Identity (`UserName`, `Email`, `Id`).

### Roles — flag enum, not table explosion

`Roles.RoleType` is a `[Flags]` enum. A user can carry `Admin | Finance | Support` as a single `int` column (`RoleFlags`) instead of three join-table rows. Bitwise checks (`HasFlag`) are O(1) and JWT-friendly.

```csharp
[Flags]
public enum RoleType {
    None = 0, Admin = 1, Customer = 2, Merchant = 4,
    Audit = 8, Finance = 16, Support = 32, CustomerRep = 64
}
```

ASP.NET Identity roles (`IdentityRole<Guid>`) are still wired in for cases where a role needs claims or hierarchy — both systems coexist.

---

## 5. Request Flow — A Sample Charge (Target Design)

This sequence describes the **intended** payment flow once the provider adapter and webhook worker land (see roadmap).

```mermaid
sequenceDiagram
    autonumber
    participant C as Client
    participant API as PaymentsController
    participant SVC as PaymentService
    participant PROV as Provider Adapter
    participant DB as PostgreSQL
    participant W as Webhook Worker

    C->>API: POST /api/intents (Idempotency-Key: abc)
    API->>SVC: CreateIntent(dto)
    SVC->>DB: INSERT PaymentIntent (status=requires_action)
    SVC-->>API: intent
    API-->>C: 201 Created { client_secret }

    C->>API: POST /api/intents/{id}/confirm
    API->>SVC: Confirm(intentId)
    SVC->>DB: INSERT PaymentAttempt
    SVC->>PROV: Charge(method, amount)
    PROV-->>SVC: providerRef, status=succeeded
    SVC->>DB: INSERT Payment, Transaction, LedgerEntries (debit/credit)
    SVC->>DB: UPDATE PaymentIntent (status=succeeded)
    SVC->>DB: INSERT EventStore (payment.succeeded)
    SVC-->>API: success
    API-->>C: 200 OK

    Note over W,DB: Async
    W->>DB: SELECT unsent WebhookDeliveries
    W->>C: POST subscriber URL
    W->>DB: UPDATE WebhookDelivery (status=delivered)
```

---

## 6. Persistence

- **Provider:** PostgreSQL via `Npgsql.EntityFrameworkCore.PostgreSQL`.
- **DbContext:** `AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>` — single context, no bounded-context split (yet).
- **Migrations:** EF Core code-first, stored in `MoniePay/Migrations/`.
- **Pooling:** `AddDbContextPool<AppDbContext>` is used so the host reuses contexts under load.

### Naming conventions

| Concept | Convention |
|---|---|
| Tables | PascalCase plural (`Users`, `PaymentIntents`) |
| Identity tables | Renamed to drop the `AspNet` prefix |
| Keys | `Guid` everywhere — easier sharding, no leaked sequence info |
| Money | `decimal Amount` + `string Currency` (ISO 4217) — never `float`/`double` |
| Timestamps | `DateTime CreatedAt` / `UpdatedAt`, UTC |

---

## 7. Project Layout

```
MoniePay/                           Solution root
├── MoniePay/                       Server host
│   ├── Program.cs                  Composition root — DI, middleware, routes
│   ├── appsettings*.json           Connection strings, JWT secrets
│   ├── Components/                 Blazor Server pages, layout, routes
│   ├── Migrations/                 EF Core migration snapshots
│   └── src/
│       ├── auth/                   User, Roles (flag enum)
│       ├── controller/             AuthController (and future controllers)
│       ├── data/                   AppDbContext
│       ├── models/                 Domain entities
│       └── services/               IdentityService, RegisterUser DTO
├── MoniePay.Client/                Blazor WebAssembly project (auto render)
├── docs/                           This document and friends
└── MoniePay.sln
```

The `src/` subfolder grouping (`auth`, `controller`, `data`, `models`, `services`) is a feature-style layout — easier to find related files when working on one capability at a time, and a natural seam if any folder grows into its own project.

---

## 8. Design Decisions — Why It Looks Like This

| Decision | Alternative considered | Why we chose this |
|---|---|---|
| Single host (API + Blazor) | Separate API + SPA repos | Smaller cognitive load; seam exists at service layer so a split is mechanical later. |
| `Guid` primary keys | `bigint` sequences | Safer for distributed inserts; no sequence enumeration leak. |
| Flag-enum `RoleFlags` on user | Identity roles only | One row per user is enough for coarse RBAC; Identity roles remain for cases that need claims. |
| Rename Identity tables | Keep `AspNet*` prefix | Schema reads like a product schema, not a framework dump — helps reviewers and DBAs. |
| Double-entry ledger | Single `balance` column | Balances cannot drift; audits are mechanical; reversals are entries, not mutations. |
| `EventStore` table | CDC / external broker | Internal projection surface without adding Kafka/etc. for the demo scope. |
| EF Core migrations in repo | Schema diffing tools | Reviewable, replayable, branch-aware. |

---

## 9. Future Extension Points

- **Provider adapters.** An `IPaymentProvider` interface with `Stripe`/`Paystack`/`Mock` implementations would let `PaymentService` stay provider-agnostic.
- **Background workers.** `IHostedService` for webhook redelivery (exponential backoff) and reconciliation.
- **Read models.** Project `EventStore` into denormalized tables (or Postgres materialized views) for dashboards.
- **Multi-tenancy.** `Customer.TenantId` already exists; adding tenant filters to `AppDbContext.SaveChanges` would isolate data per merchant.
- **Outbox pattern.** Move outbound webhooks to an `Outbox` table written in the same transaction as the domain change, drained by the worker — guarantees at-least-once delivery without distributed transactions.

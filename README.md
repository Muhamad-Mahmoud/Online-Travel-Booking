# 🌐 Online Travel Booking System

A scalable, multi-tenant travel booking platform developed in **ASP.NET Core 9**. It provides a unified reservation engine for **Hotels, Flights, Tours, and Car Rentals**. The system exposes a **RESTful Client API** alongside an integrated **Razor Pages Admin Dashboard** for comprehensive entity management.

Engineered with **Domain-Driven Design (DDD)** and **Clean Architecture**, the solution implements CQRS principles and relies on decentralized state management. It ensures transactional safety, data integrity, and deterministic execution flow.

---

## 🎯 Domain Capabilities

The architecture isolates category-specific domain logic across four heavily-modeled verticals beneath a single abstract booking system.

### ⚙️ The Booking Engine
- **Unified Abstraction:** Handles diverse reservation types seamlessly while executing context-specific validation.
- **Strategy Pattern Execution:** Injects dynamic pricing and allocation logic at runtime depending on the reservation category.
- **Lazy Expiration Lifecycle:** State transitions (Pending → Confirmed / Cancelled / Expired) are evaluated deterministically on-read, eliminating the need for volatile background polling jobs.
- **Invariant Enforcement:** Strict domain rules physically prevent invalid state transitions, such as double-booking physical resources.

### 🏤 Hotels & Accommodations
- **Granular Inventory Control:** Management over individual room availability and seasonal capacity fluctuations.
- **Dynamic Pricing Matrices:** Algorithm-driven rate adjustments based on tier, capacity, and seasonality.
- **Automated Aggregation:** In-memory calculations for aggregate review scores and metadata relationships.

### 🛫 Flight Integration
- **Relational Flight Scheduling:** Granular dependencies mapping flights to airports, aircraft, gates, and standard IATA codes.
- **Multi-Class Allocation:** Deep inventory separation across cabin classifications (Economy, Business, First).
- **Flexible Ancillaries:** Dynamic application of baggage rules and variable cancellation fee structures.

### 🧭 Tours & Itineraries
- **Geospatial Tracking:** Uses **NetTopologySuite** to map precise coordinates for multi-day travel routes and starting locations.
- **Tiered Profit Logic:** Highly segmented demographic pricing models (e.g., Adult, Child).

### 🚘 Fleet Operations
- **Geospatial Depot Management:** Distance and availability tracking for car rentals based on pick-up metadata.
- **Time-Matrix Pricing:** Scalable daily cost functions governed by vehicle classification and required durations.

---

## 🏗️ Architecture & Core Infrastructure

The solution enforces strict dependency inversion across 4 decoupled layers:

| Layer | Responsibility | Details |
|-------|----------------|---------|
| **Domain** | Core Business Logic | Entities, Value Objects (`Money`, `DateTimeRange`), Enums, Exceptions |
| **Application** | Use Cases & Orchestration | CQRS (MediatR), Fluent Validation, Interfaces, Service Contracts |
| **Infrastructure** | Implementations | EF Core, NetTopologySuite, SMTP, Stripe Webhooks, Identity |
| **Presentation** | External Interfaces | REST Controllers, Identity Endpoints, MVC Razor Views |

### 🧠 Advanced Architectural Capabilities
- **Vertical Slice & Polymorphic Feature Models:** The `Application` layer discards traditional horizontal slicing in favor of cohesive **Feature Verticals** (e.g., `Features/Bookings`). Instead of scattering anemic DTOs, each slice encapsulates rich, inheritance-driven **Feature Models** tailored exactly to the request context (e.g., polymorphism between `BookingResponse` and `AdminBookingResponse`). This ensures tight aggregation, strict encapsulation, and zero cross-boundary DTO leakage.
- **Pre-Execution Pipeline Validation:** A MediatR `ValidationBehavior` automatically intercepts and evaluates injected `FluentValidation` rules before any command/query reaches its designated handler.
- **Specification-Driven Repository:** An `IGenericRepository<T>` built on the **Specification Pattern**. It encapsulates highly-complex, chainable LINQ trees independently of the database context, removing query duplication.
- **Global Error Standardization (RFC 7807):** A centralized `ExceptionMiddleware` catches domain violations and unhandled errors, stripping stack traces and returning predictable `ProblemDetails` objects.
- **Result Pattern Determinism:** Execution flow strictly relies on robust `Result<T>` mapping rather than volatile `try/catch` closures inside controllers.

---

## ⚡ Key System Components

### 💳 Stripe Infrastructure
- **Webhook Resiliency:** Strict payload and event signature verification.
- **Concurrency Protection:** Real-time event deduplication prevents race conditions during overlapping network latency.
- **Fault-Tolerant Persistence:** Ensures initial payment intents and responses are asynchronously logged securely.

### 🔒 Access Control & Identity
- **Hybrid Auth Standard:** Fuses stateful ASP.NET Identity with stateless JWT integration (HMAC-SHA256) and automated Google OAuth 2.0 mapping.
- **Role-Based Execution (RBAC):** Restrictive operation boundaries dividing client interactions from the Admin Dashboard.
- **Data Preservation:** Critical operational data is protected by global Soft Deletion rules, maintaining historical audit accuracy.

### 📈 Administrative Operations (Razor MVC)
- **High-Performance Reporting:** Parallelized background querying for aggregate transaction and distribution analytics.
- **Batched Generation:** Efficient memory streaming for large-scale CSV log exports.

---

## 🛠 Tech Stack Overview

- **SDK/Runtime:** .NET 9
- **Data Persistence:** Entity Framework Core 9 (SQL Server) + NetTopologySuite
- **Orchestration:** MediatR 12 + FluentValidation 12
- **Mapping:** Mapster
- **External Dependencies:** Stripe API, MailKit (SMTP), Serilog (Structured Logging)

---

## 🚀 Environment Setup

1. **Clone & Setup:**
   ```bash
   git clone https://github.com/yourusername/OnlineTravel.git
   cd OnlineTravel
   ```

2. **Configuration:**
   Update the `appsettings.json` within the Application Entry points:
   - Provide the SQL Server Connection String.
   - Insert Stripe Webhook/Secret keys.
   - Configure SMTP credentials and JWT generation secrets.

3. **Database Initialization & Execution:**
   ```bash
   dotnet run --project OnlineTravel.Mvc
   ```
   *The system relies on EF Core Migrations to automatically build, seed, and map standard admin roles on first execution.*

---
*Engineered for scale, consistency, and absolute integrity.*

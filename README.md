# 🌍 Online Travel Booking System

A travel booking platform built with **ASP.NET Core 9** that supports Hotels, Flights, Tours, and Car Rentals — exposing **REST APIs** for client applications and providing a built-in **Razor-based Admin Dashboard**.

The system is designed with clear separation of concerns, centralized business logic, and secure payment processing via **Stripe**.

---

## 🏗 Architecture

The solution follows a 4-layer structure with dependencies flowing inward:

| Layer | Project | Role |
|-------|---------|------|
| **Domain** | `OnlineTravel.Domain` | Entities, Value Objects, Enums, business rules |
| **Application** | `OnlineTravel.Application` | Use cases (Commands/Queries), Interfaces, Validators |
| **Infrastructure** | `OnlineTravel.Infrastructure` | Database, Auth, Payments, Email, File Storage |
| **Presentation** | `OnlineTravel.Api` + `OnlineTravel.Mvc` | API Controllers + Admin Dashboard (Razor Views) |

Each use case lives in its own feature folder with its Command/Query, Handler, Validator, and DTOs.

---

## 🛠 Tech Stack

| | |
|--|--|
| .NET 9 / ASP.NET Core 9 | Entity Framework Core 9 (SQL Server) |
| MediatR 12 | FluentValidation 12 |
| ASP.NET Identity + JWT | Google OAuth 2.0 |
| Stripe (Checkout + Webhooks) | Serilog (structured logging) |
| Mapster + AutoMapper | MailKit (SMTP) |
| NetTopologySuite (geospatial) | Swagger UI |

---

## ⭐ Features

### Bookings
- Multi-category bookings — Hotels, Flights, Tours, Cars
- Each category has its own booking strategy (validation + pricing)
- Booking lifecycle: PendingPayment → Confirmed / Cancelled / Expired
- 15-minute payment window with lazy expiration on read
- Overlapping reservation detection for hotel rooms
- Unique booking references (`BK-XXXXXXXX`)
- Paginated history with search and status filtering

### Payments (Stripe)
- Checkout Sessions for secure payment flow
- Webhook handling with signature verification
- Event deduplication to prevent double-processing
- Booking is saved even if Stripe session creation fails

### Auth
- JWT authentication with role-based access (User / Admin)
- Google OAuth external login
- Email confirmation, forgot/reset password
- Soft account deletion with 30-day restore window
- Roles seeded automatically on startup

### Admin Dashboard (MVC)
- Revenue analytics, category distribution, recent bookings
- Booking management — search, filter, details, CSV export (batched)
- Tour management — CRUD, activities, images, price tiers, coordinates
- Hotel management — rooms, seasonal pricing, availability
- Parallel query execution for dashboard performance

### Domain Modules
- **Hotels** — rooms, seasonal pricing, availability tracking, reviews, auto-rating
- **Flights** — carriers, airports, seats, fares, scheduling
- **Tours** — activities, price tiers, image galleries, geospatial coordinates
- **Cars** — brands, pricing tiers, geospatial location, cancellation policies
- **Favorites** — wishlist functionality

### Other
- File upload with organized folder structure
- Health check endpoint (`/health`)
- Global error handling with consistent response format

---

## 🔒 Security

- JWT (HMAC-SHA256) with configurable expiry
- Role-based authorization on admin endpoints
- Email confirmation required before login
- Stripe webhook signature validation + deduplication
- Exception middleware hides internals in production
- FluentValidation on all incoming commands
- Domain-level guard clauses on entities

---

## 📡 API Overview

**REST API** — `/api/[controller]`

| Endpoint Group | Operations |
|----------------|-----------|
| Auth | Register, Login, Google Login, Email Confirm, Password Reset, Account Delete/Restore |
| Bookings | Create, Cancel, Get Mine, Get by ID, Get All (Admin) |
| Payments | Stripe Webhook |
| Hotels / Flights / Tours / Cars | Search, CRUD |
| CarBrands / CarPricingTiers | CRUD |
| Airports / Carriers | CRUD |
| Favorites | Add, Remove, List |
| Upload | Image upload |

**Admin Dashboard** — `/Admin/[action]`

Dashboard, Bookings, Tours, Hotels, Flights, Car Brands — full management UI.

**Response format:**
```json
{
  "statusCode": 200,
  "message": "Success",
  "isSuccess": true,
  "data": { }
}
```

Errors follow RFC 7807 Problem Details.

---

## 🚀 How to Run

**Prerequisites:** .NET 9 SDK, SQL Server, Stripe CLI *(optional)*

1. Clone the repo
2. Update `appsettings.json` — connection string, Stripe keys, email settings, JWT secret
3. Run:
   ```bash
   cd OnlineTravel
   dotnet run
   ```
4. Open:
   - Swagger: `https://localhost:5091/swagger`
   - Admin: `https://localhost:5091/Admin`
   - Health: `https://localhost:5091/health`

> Database is created, migrated, and seeded automatically on first startup.

---

## 🎨 Patterns Used

| Pattern | Usage |
|---------|-------|
| Repository + Unit of Work | Data access with transaction support |
| Specification | Composable query building |
| Strategy | Per-category booking processing |
| Result | Error handling without exceptions |
| Value Objects | `Money`, `Address`, `DateTimeRange`, `BookingReference` |
| Factory Method | `BookingEntity.Create()`, `ProcessedWebhookEvent.Create()` |
| Pipeline Behavior | Automatic validation before handlers |
| Soft Delete | `SoftDeletableEntity` with `DeletedAt` |
| Lazy Expiration | Expired bookings marked on read |
| Options | Typed configuration (`JwtOptions`, `StripeOptions`, `EmailSettings`) |

---

*Built with ASP.NET Core 9 and modern backend practices.*

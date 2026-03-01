# 🌐 Online Travel Booking System

> A travel booking platform built with **ASP.NET Core 9**, providing a unified system for **Hotels, Flights, Tours, and Car Rentals**. It features a **RESTful Client API** and a **Razor Pages Admin Dashboard** for managing business operations.

Built on **Clean Architecture**, the solution follows CQRS principles to ensure a maintainable and scalable codebase. It focuses on transactional safety and clear separation of concerns.

---

## 🎯 System Features

The system manages different booking categories through a shared core, while allowing for category-specific logic where needed.

### ⚙️ Booking Engine
- **Unified Processing:** Handles different reservation types through a consistent workflow.
- **Dynamic Pricing:** Implements pricing strategies that adjust based on reservation category and timing.
- **Automated Expiration:** Automatically handles booking lifecycles (Pending, Confirmed, Cancelled, Expired) without complex background jobs.

### 🏤 Hotels & Accommodations
- **Inventory Control:** Manage room availability and seasonal capacity.
- **Flexible Pricing:** Set rates based on room types, demand, and seasonality.
- **Reviews & Ratings:** Automated calculation of average scores and customer feedback management.

### 🛫 Flight Management
- **Scheduling:** Links flights to airports, carriers, and aircraft using standard industry codes.
- **Cabin Classes:** Support for Economy, Business, and First-class seat allocation.
- **Baggage & Fees:** Dynamic management of baggage rules and cancellation policies.

### 🧭 Tours & Itineraries
- **Location Mapping:** Uses **NetTopologySuite** for accurate GPS coordinates of tour routes and meeting points.
- **Tiered Pricing:** Support for different price points for adults, children, and other groups.

### 🚘 Car Rentals
- **Depot Management:** Track vehicle availability across different pick-up locations.
- **Duration-Based Pricing:** Calculate rental costs based on vehicle class and time period.

---

## 🏗️ Technical Architecture

The project is structured into four layers, enforcing a clear separation of business logic from infrastructure.

| Layer | Responsibility | Key Components |
|-------|----------------|----------------|
| **Domain** | Business Core | Entities, Value Objects, Enums, Domain Exceptions |
| **Application** | Use Cases | MediatR Handlers, Validators, Service Interfaces |
| **Infrastructure** | Implementations | EF Core, NetTopologySuite, SMTP, Stripe, Identity |
| **Presentation** | API & UI | REST Controllers, Identity Endpoints, Razor Pages |

### 🧠 Key Patterns

- **Vertical Feature Slicing:** 
  The Application layer is organized by feature rather than type. Each feature encapsulates its own logic, DTOs, and mappings, making the system easier to navigate and maintain.

- **Pipeline Validation:** 
  Validation rules are automatically executed using MediatR behaviors, ensuring only valid data reaches the business handlers.

- **Specification Pattern:** 
  Queries are abstracted into reusable Specification classes, keeping the repository clean and avoiding duplicate query logic across the application.

- **Standardized Error Handling:** 
  A global middleware handles exceptions and returns consistent error responses across all API endpoints.

- **Result Pattern:** 
  Uses a `Result<T>` pattern for internal communication, making error handling more explicit and reducing the need for exception-based flow control.

---

## ⚡ Core Integration

### 💳 Payment Processing (Stripe)
- **Secure Webhooks:** Verified event processing to handle payments reliably.
- **Race Condition Protection:** Handles overlapping events to ensure data consistency.
- **Audit Logging:** Asynchronous logging of all payment attempts and responses.

### 🔒 Security & Identity
- **Flexible Authentication:** Combines ASP.NET Identity with JWT for API access and Google OAuth for social login.
- **Role-Based Access:** Distinct permissions for clients and administrators.
- **Soft Deletion:** Safely manage data removal without losing historical records.

### 📈 Admin Dashboard
- **Performance Analytics:** Real-time reporting on bookings and revenue distribution.
- **Data Export:** Support for exporting operational logs and reports to CSV.

---

## 🛠 Technology Stack

- **Framework:** .NET 9
- **Database:** SQL Server + Entity Framework Core 9
- **Libraries:** MediatR, FluentValidation, Mapster
- **External:** Stripe API, MailKit, Serilog

---

## 🚀 Getting Started

1. **Clone the project:**
   ```bash
   git clone https://github.com/yourusername/OnlineTravel.git
   ```

2. **Configure:**
   Update `appsettings.json` with your:
   - SQL Server Connection String
   - Stripe API Keys
   - SMTP and JWT settings

3. **Run:**
   ```bash
   dotnet run --project OnlineTravel.Mvc
   ```
   The database will be automatically created and seeded on the first run.

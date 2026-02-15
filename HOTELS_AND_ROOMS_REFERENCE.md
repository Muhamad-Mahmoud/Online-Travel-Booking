# مرجع Hotels & Rooms — Endpoints و Controllers وطريقة الاختبار

## 1) أين الـ Controllers والـ Endpoints

### Controller: **HotelsController**  
**الملف:** `OnlineTravelBookingTeamB/Controllers/HotelsController.cs`  
**Base route:** `api/v1/hotels`

| Method | Route | Action | الوصف |
|--------|--------|--------|--------|
| GET | `api/v1/hotels` | SearchHotels | بحث فنادق (مدينة، تواريخ، سعر، نجوم، موقع، ترقيم) |
| GET | `api/v1/hotels/{id}` | GetHotelDetails | تفاصيل فندق واحد |
| GET | `api/v1/hotels/{id}/rooms` | GetHotelRooms | غرف الفندق (اختياري: checkin, checkout) |
| POST | `api/v1/hotels/{id}/reviews` | AddReview | إضافة تقييم (rating 1–5) |

---

### Controller: **AdminHotelsController**  
**الملف:** `OnlineTravelBookingTeamB/Controllers/AdminHotelsController.cs`  
**Base route:** `api/v1/admin`

| Method | Route | Action | الوصف |
|--------|--------|--------|--------|
| POST | `api/v1/admin/hotels` | CreateHotel | إنشاء فندق |
| PUT | `api/v1/admin/hotels/{id}` | UpdateHotel | تحديث فندق |
| POST | `api/v1/admin/hotels/{hotelId}/rooms` | AddRoom | إضافة غرفة لفندق |
| PUT | `api/v1/admin/rooms/{id}` | EditRoom | تعديل غرفة |
| PUT | `api/v1/admin/rooms/{id}/availability` | ManageAvailability | تعيين توفر الغرفة لفترة |
| POST | `api/v1/admin/rooms/{id}/seasonal-pricing` | ConfigureSeasonalPricing | إضافة سعر موسمي للغرفة |

---

## 2) ربط الـ Endpoints مع الـ Application (Hotels)

| Endpoint | Query / Command | Handler |
|----------|------------------|---------|
| GET /api/v1/hotels | SearchHotelsQuery | SearchHotelsQueryHandler |
| GET /api/v1/hotels/{id} | GetHotelDetailsQuery | GetHotelDetailsQueryHandler |
| GET /api/v1/hotels/{id}/rooms | GetHotelRoomsQuery | GetHotelRoomsQueryHandler |
| POST /api/v1/hotels/{id}/reviews | AddReviewCommand | AddReviewCommandHandler |
| POST /api/v1/admin/hotels | CreateHotelCommand | CreateHotelCommandHandler |
| PUT /api/v1/admin/hotels/{id} | UpdateHotelCommand | UpdateHotelCommandHandler |
| POST /api/v1/admin/hotels/{id}/rooms | AddRoomCommand | AddRoomCommandHandler |
| PUT /api/v1/admin/rooms/{id} | EditRoomCommand | EditRoomCommandHandler |
| PUT /api/v1/admin/rooms/{id}/availability | ManageAvailabilityCommand | ManageAvailabilityCommandHandler |
| POST /api/v1/admin/rooms/{id}/seasonal-pricing | ConfigureSeasonalPricingCommand | ConfigureSeasonalPricingCommandHandler |

كل الـ Commands/Queries والـ Handlers والـ Validators موجودة تحت:  
`OnlineTravel.Application/Hotels/` (Admin و Public و Specifications و Dtos و Common).

---

## 3) كيف تختبر كل الـ Endpoints

### الطريقة 1: Swagger (مستحسنة)

1. شغّل المشروع:
   ```bash
   cd OnlineTravelBookingTeamB
   dotnet run
   ```
2. افتح المتصفح: **https://localhost:5091/swagger** (أو الـ URL اللي في `launchSettings.json`).
3. من Swagger تقدر تجرب كل الـ endpoints وتشوف الـ request/response.

### الطريقة 2: ملف HTTP (HotelsAndRooms.http)

1. افتح الملف **`HotelsAndRooms.http`** في جذر المشروع (نفس مستوى الـ .sln).
2. تأكد أن الـ Base URL مطابق للبورت اللي المشروع شغال عليه (مثلاً `https://localhost:5091`).
3. استبدل كل **REPLACE_WITH_HOTEL_ID** و **REPLACE_WITH_ROOM_ID** و **REPLACE_WITH_USER_ID** بـ GUIDs حقيقية من الـ DB أو من استجابة سابقة (مثلاً بعد Create Hotel أو من `/api/dev/seed-data` إن وُجد).
4. اضغط "Send Request" فوق كل طلب في الـ .http.

- **ملاحظة:** في Create/Update Hotel الـ `checkInTime` و `checkOutTime` من نوع TimeOnly؛ في JSON استخدم صيغة مثل `"14:00"` أو `"15:30:00"`.

### الطريقة 3: ترتيب اختبار منطقي (سكنداريو)

1. **إنشاء فندق:**  
   `POST /api/v1/admin/hotels` بالـ body المناسب من الـ .http → خذ `id` الفندق من الـ response.
2. **تحديث فندق:**  
   `PUT /api/v1/admin/hotels/{id}` بنفس الـ id.
3. **إضافة غرفة:**  
   `POST /api/v1/admin/hotels/{hotelId}/rooms` (استخدم نفس الـ hotelId) → خذ `id` الغرفة.
4. **تعديل غرفة:**  
   `PUT /api/v1/admin/rooms/{roomId}`.
5. **توفر الغرفة:**  
   `PUT /api/v1/admin/rooms/{roomId}/availability` (فترة + isAvailable).
6. **سعر موسمي:**  
   `POST /api/v1/admin/rooms/{roomId}/seasonal-pricing`.
7. **بحث:**  
   `GET /api/v1/hotels` و `GET /api/v1/hotels?City=...&CheckIn=...&CheckOut=...`.
8. **تفاصيل فندق:**  
   `GET /api/v1/hotels/{id}`.
9. **غرف فندق:**  
   `GET /api/v1/hotels/{id}/rooms` و `GET .../rooms?checkin=...&checkout=...`.
10. **تقييم:**  
    `POST /api/v1/hotels/{id}/reviews` (مع userId من الـ DB أو seed).

---

## 4) فحص المكونات الخاصة بـ Hotel & Room

### Domain — Entities (Hotels & Bookings ValueObjects)

- **الموقع:** `OnlineTravel.Domain/Entities/Hotels/`
- **الكيانات:**  
  `Hotel`, `Room`, `SeasonalPrice`, `RoomAvailability`, و (إن وُجدت) `HotelGallery`, `RoomPhoto`.
- **Value Objects (مشتركة):**  
  `OnlineTravel.Domain/Entities/_Shared/ValueObjects/` (مثل Address, Money, DateRange, ContactInfo, TimeRange, Url, etc.)  
  و **Bookings:** `OnlineTravel.Domain/Entities/Bookings/ValueObjects/` (مثل BookingReference).

### Application — Interfaces

- **IUnitOfWork:**  
  `OnlineTravel.Application/Interfaces/Persistence/IUnitOfWork.cs`  
  - يعرّف: `Hotels`, `Rooms`, `Repository<T>()`, `SaveChangesAsync`, `Complete`, وبدء/commit/rollback المعاملات.
- **IHotelRepository:**  
  `OnlineTravel.Application/Interfaces/Persistence/IHotelRepository.cs`  
  - يرث من `IGenericRepository<Hotel>` + `GetBySlugAsync`, `SlugExistsAsync`, `GetWithRoomsAsync`, `GetWithReviewsAsync`.
- **IRoomRepository:**  
  `OnlineTravel.Application/Interfaces/Persistence/IRoomRepository.cs`  
  - يرث من `IGenericRepository<Room>` + `GetWithAvailabilityAsync`, `GetWithSeasonalPricesAsync`, `RoomNumberExistsInHotelAsync`, `GetHotelRoomsAsync`.
- **IGenericRepository&lt;T&gt;:**  
  `OnlineTravel.Application/Interfaces/Persistence/IGenericRepository.cs`  
  - عمليات عامة: GetByIdAsync, GetAllAsync, ListAsync مع Specification, AddAsync, Update, Delete, FindAsync, إلخ.
- **IFileService:**  
  `OnlineTravel.Application/Interfaces/Services/IFileService.cs`  
  - `UploadFileAsync`, `DeleteFile`, `GetFileUrl` — **حالياً لا يُستخدم في أي من Hotel/Room endpoints أعلاه**؛ يمكن لاحقاً ربطه برفع صورة الفندق أو صور الغرف إذا أضفت endpoint مثل `UploadRoomImage` أو `UploadHotelImage`.

### Infrastructure — UnitOfWork و Repositories

- **UnitOfWork:**  
  `OnlineTravel.Infrastructure/Persistence/UnitOfWork/UnitOfWork.cs`  
  - ينشئ `HotelRepository` و `RoomRepository` ويعرضهم كـ `Hotels` و `Rooms`؛ يستخدم `GenericRepository<T>` لبقية الـ entities عبر `Repository<T>()`.
- **HotelRepository:**  
  `OnlineTravel.Infrastructure/Persistence/Repositories/HotelRepository.cs`  
  - تنفيذ `GetBySlugAsync`, `GetWithRoomsAsync`, `GetWithReviewsAsync` مع Eager Loading (Rooms → SeasonalPrices, RoomAvailabilities, Bookings + Reviews).
- **RoomRepository:**  
  `OnlineTravel.Infrastructure/Persistence/Repositories/RoomRepository.cs`  
  - تنفيذ `GetWithAvailabilityAsync`, `GetWithSeasonalPricesAsync`, `GetHotelRoomsAsync`, `RoomNumberExistsInHotelAsync` مع Eager Loading.

### Application — Hotels Module

- **الموقع:** `OnlineTravel.Application/Hotels/`
- **Admin:**  
  CreateHotelCommand, UpdateHotel, AddRoom, EditRoom, ManageAvailability, ConfigureSeasonalPricing (Command + Handler + Validator لكلٍ منها حيث مطلوب).
- **Public:**  
  SearchHotels (Query + Handler + Validator), GetHotelDetails, GetHotelRooms, AddReview (Command + Handler + Validator).
- **Specifications:**  
  `HotelSearchSpecification` (فلترة + ترتيب + ترقيم صفحات).
- **DTOs و Common:**  
  تحت `Hotels/Dtos/` و `Hotels/Common/` (Result, PagedResult, إلخ).

---

## 5) ملخص سريع

- **كل الـ Endpoints والـ Controllers** الخاصة بـ Hotel و Room موجودة في:  
  **HotelsController** (`api/v1/hotels`) و **AdminHotelsController** (`api/v1/admin`).
- **كيف تختبر:**  
  Swagger من المتصفح، أو استخدام ملف **HotelsAndRooms.http** بعد استبدال الـ IDs وتشغيل المشروع.
- **كل ما يخص Hotel & Room** من Domain و Interfaces و UnitOfWork و Repositories و Application/Hotels تمت مراجعته أعلاه؛ **IFileService** موجود لكن غير مستخدم حالياً في أي من هذه الـ endpoints ويمكن ربطه لاحقاً برفع الصور.

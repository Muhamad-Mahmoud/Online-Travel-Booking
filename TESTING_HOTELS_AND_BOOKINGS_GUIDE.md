# دليل اختبار Admin Hotels و Bookings — Swagger و Postman (خطوة بخطوة)

## 1) تشغيل المشروع والحصول على الـ IDs

### تشغيل الـ API
```bash
cd OnlineTravelBookingTeamB
dotnet run
```
- الـ API عادةً يعمل على: **https://localhost:5091** (أو الـ URL الظاهر في الـ Console).
- تأكد أن الـ Database معمول لها migrate والـ seed شغال (مثلاً من `SeedDatabaseAsync`).

### الحصول على بيانات للتجربة (User, Category, Hotel, Room)
1. افتح في المتصفح أو Postman:
   - **GET** `https://localhost:5091/api/Dev/seed-data`
2. الـ Response يعطيك شيء مثل:
   - `customer.id` → **UserId** للـ Booking
   - `categories` → اختر الـ category اللي `type` بتاعها **Hotel** وخذ **Id** → **CategoryId**
   - `sampleHotel` → **Id** = HotelId، و **roomId** = أول غرفة → **ItemId** (Room Id)

احفظ:
- **UserId** من `customer.id`
- **CategoryId** (Hotel) من `categories`
- **HotelId** و **RoomId** من `sampleHotel`

---

## 2) الاختبار بـ Swagger

### فتح Swagger
- افتح المتصفح: **https://localhost:5091**
- (أو **https://localhost:5091/swagger** حسب إعداد الـ `RoutePrefix` في المشروع.)

### ترتيب التجربة المنطقي

#### أ) رفع الصور أولاً (اختياري لكن مفيد للـ MainImage و Room Photos)

- من Swagger ادخل على **Upload** (أو الـ Controller اللي فيه رفع الصور).

1. **رفع صورة فندق**
   - اختر **POST** `/api/v1/Upload/hotel-image`
   - اضغط **Try it out**
   - في **file**: اختر ملف صورة من جهازك (JPG/PNG)
   - اضغط **Execute**
   - من الـ Response انسخ الـ **url** (مثلاً `https://localhost:5091/uploads/hotels/xxx.jpg`) واستخدمها في **mainImage** عند إنشاء/تحديث الفندق.

2. **رفع صورة غرفة**
   - **POST** `/api/v1/Upload/room-image`
   - نفس الخطوات، ثم انسخ الـ **url** لاستخدامها في **photos** عند إضافة غرفة.

#### ب) Admin — إنشاء فندق

1. ادخل على **AdminHotels** (أو الـ section اللي فيه Admin endpoints).
2. **POST** `/api/v1/admin/hotels`
   - **Try it out**
   - Body (application/json) — استخدم مثال مثل التالي (والـ mainImage يمكن أن تكون الـ URL اللي جبتها من رفع الصورة، أو أي رابط صورة):

```json
{
  "name": "فندق التجربة",
  "slug": "test-hotel-cairo",
  "description": "فندق للتجربة في القاهرة.",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "street": "شارع التحرير 123",
  "city": "Cairo",
  "state": "Cairo",
  "country": "Egypt",
  "postalCode": "11511",
  "mainImage": "https://example.com/hotel.jpg",
  "gallery": [],
  "checkInTime": "14:00",
  "checkOutTime": "11:00",
  "cancellationPolicy": "إلغاء مجاني قبل 24 ساعة.",
  "contactPhone": "+201234567890",
  "contactEmail": "info@testhotel.com",
  "website": "https://testhotel.com"
}
```

- ملاحظة: **checkInTime** و **checkOutTime** بصيغة **"HH:mm"** (مثل "14:00", "11:00").
- بعد **Execute** انسخ من الـ Response الـ **id** الخاص بالفندق → هذا هو **HotelId** للخطوات التالية.

#### ج) Admin — تحديث الفندق

- **PUT** `/api/v1/admin/hotels/{id}`
- ضع في **id** نفس **HotelId** اللي حفظته.
- Body مثل Create لكن بدون **slug** و **gallery** إن لم يكونا مطلوبين (وفق الـ UpdateHotelCommand)، مثلاً:

```json
{
  "name": "فندق التجربة (محدّث)",
  "description": "وصف محدّث.",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "street": "شارع التحرير 123",
  "city": "Cairo",
  "state": "Cairo",
  "country": "Egypt",
  "postalCode": "11511",
  "mainImage": "https://example.com/hotel.jpg",
  "checkInTime": "15:00",
  "checkOutTime": "12:00",
  "cancellationPolicy": "إلغاء مجاني قبل 48 ساعة.",
  "contactPhone": "+201234567890",
  "contactEmail": "info@testhotel.com",
  "website": "https://testhotel.com"
}
```

#### د) Admin — إضافة غرفة

- **POST** `/api/v1/admin/hotels/{hotelId}/rooms`
- **hotelId** = نفس الـ Hotel Id.

Body مثال (صور الغرفة يمكن أن تكون URLs من رفع room-image):

```json
{
  "hotelId": "ضع-هنا-HotelId-كـ-GUID",
  "roomNumber": "101",
  "name": "غرفة مزدوجة",
  "description": "غرفة مزدوجة مريحة.",
  "basePricePerNight": 99.99,
  "photos": [
    { "url": "https://localhost:5091/uploads/rooms/xxx.jpg", "alt": "غرفة 101" }
  ]
}
```

- يمكن ترك **photos** مصفوفة فارغة `[]` أو حذفها إن كانت اختيارية.
- من الـ Response انسخ **id** الغرفة → **RoomId** للخطوات التالية.

#### هـ) Admin — تعديل غرفة

- **PUT** `/api/v1/admin/rooms/{id}`
- **id** = Room Id.

```json
{
  "id": "نفس-RoomId",
  "name": "غرفة مزدوجة (محدّثة)",
  "description": "وصف محدّث.",
  "basePricePerNight": 119.99
}
```

#### و) Admin — توفر الغرفة

- **PUT** `/api/v1/admin/rooms/{id}/availability`
- **id** = Room Id.

```json
{
  "roomId": "نفس-RoomId",
  "startDate": "2026-03-01",
  "endDate": "2026-03-31",
  "isAvailable": true
}
```

- التواريخ بصيغة **YYYY-MM-DD**.

#### ز) Admin — سعر موسمي للغرفة

- **POST** `/api/v1/admin/rooms/{id}/seasonal-pricing`
- **id** = Room Id.

```json
{
  "roomId": "نفس-RoomId",
  "startDate": "2026-06-01",
  "endDate": "2026-08-31",
  "price": 149.99
}
```

---

#### ح) Bookings — إنشاء حجز (فندق فقط)

- ادخل على **Bookings**.
- **POST** `/api/v1/bookings`

Body (استخدم الـ IDs من seed-data أو من الخطوات السابقة):

```json
{
  "userId": "ضع-GUID-المستخدم-من-seed-data",
  "categoryId": "ضع-GUID-فئة-Hotel-من-seed-data",
  "itemId": "ضع-GUID-الغرفة-RoomId",
  "stayRange": {
    "start": "2026-03-01T00:00:00",
    "end": "2026-03-05T00:00:00"
  }
}
```

- **stayRange.start** و **stayRange.end** بصيغة **DateTime** (ISO): `YYYY-MM-DDTHH:mm:ss` أو `YYYY-MM-DD`.
- من الـ Response انسخ الـ **booking Id** (GUID) لاستخدامه في GET.

#### ط) Bookings — عرض حجز بالـ Id

- **GET** `/api/v1/bookings/{id}`
- ضع في **id** الـ Booking Id اللي حصلت عليه من POST.
- الـ Response يعطيك تفاصيل الحجز (مرجع، مستخدم، سعر، تواريخ، تفاصيل الغرفة إن وُجدت).

---

## 3) الاختبار بـ Postman

### استيراد الـ Collection
1. افتح Postman.
2. **Import** → اختر الملف **Postman_Collection_Hotels_And_Bookings.json** من جذر المشروع.
3. ستظهر مجموعة "Hotels & Bookings API" فيها كل الطلبات بالترتيب.

### إعداد المتغيرات (Variables)
- من الـ Collection اضغط على **Variables**.
- **baseUrl**: غيّره إذا البورت مختلف (مثلاً `https://localhost:5091`).
- **userId** و **categoryId**: بعد تنفيذ الطلب **0 - Seed Data** انسخ القيم من الـ response وضعها هنا.
- **hotelId**: بعد **Admin - Create Hotel** انسخ الـ `id` من الـ response.
- **roomId**: بعد **Admin - Add Room** انسخ الـ `id` من الـ response.
- **bookingId**: بعد **Bookings - Create** انسخ الـ `id` (أو الـ value اللي يرجع في الـ body) من الـ response.

### إعداد البيئة (اختياري)
بدلاً من متغيرات الـ Collection يمكنك إنشاء **Environment** بنفس الأسماء:
- **baseUrl** = `https://localhost:5091`
- **hotelId** = (تعبّيه بعد إنشاء الفندق)
- **roomId** = (بعد إضافة الغرفة)
- **bookingId** = (بعد إنشاء الحجز)
- **userId**، **categoryId** من seed-data

### ترتيب الطلبات (بنفس المنطق السابق)

1. **GET** `{{baseUrl}}/api/Dev/seed-data`  
   → احفظ الـ IDs كما في قسم Swagger.

2. **POST** `{{baseUrl}}/api/v1/Upload/hotel-image`  
   - Tab **Body** → **form-data**
   - Key: **file** (نوع File) → اختر صورة
   - Send → انسخ **url** من الـ response.

3. **POST** `{{baseUrl}}/api/v1/admin/hotels`  
   - **Body** → **raw** → **JSON**  
   - الصق نفس الـ JSON المستخدم في Swagger لإنشاء الفندق (مع **mainImage** = الـ url من الخطوة 2 إن أردت).
   - من الـ response انسخ **id** وضعه في **hotelId** في الـ Environment.

4. **PUT** `{{baseUrl}}/api/v1/admin/hotels/{{hotelId}}`  
   - Body → raw → JSON (نفس نموذج التحديث أعلاه).

5. **POST** `{{baseUrl}}/api/v1/admin/hotels/{{hotelId}}/rooms`  
   - Body → raw → JSON (نموذج Add Room أعلاه، ويمكن استخدام **mainImage** أو **room image url** في **photos[].url**).
   - من الـ response انسخ **id** الغرفة → **roomId**.

6. **PUT** `{{baseUrl}}/api/v1/admin/rooms/{{roomId}}`  
   - Body → raw → JSON (Edit Room).

7. **PUT** `{{baseUrl}}/api/v1/admin/rooms/{{roomId}}/availability`  
   - Body → raw → JSON (Manage Availability).

8. **POST** `{{baseUrl}}/api/v1/admin/rooms/{{roomId}}/seasonal-pricing`  
   - Body → raw → JSON (Seasonal Pricing).

9. **POST** `{{baseUrl}}/api/v1/bookings`  
   - Body → raw → JSON (Create Booking مع **userId**, **categoryId**, **itemId** = roomId، و **stayRange**).
   - من الـ response انسخ الـ booking **id** → **bookingId**.

10. **GET** `{{baseUrl}}/api/v1/bookings/{{bookingId}}`  
    → عرض تفاصيل الحجز.

---

## 4) رفع الصور — بالتفصيل

### في Swagger
- الـ endpoint لصورة الفندق: **POST** `/api/v1/Upload/hotel-image`
- الـ endpoint لصورة الغرفة: **POST** `/api/v1/Upload/room-image`
- في الاثنين: لا تختار **Content-Type application/json**.
- استخدم الـ **file** parameter واختر ملف من جهازك (مثلاً JPG أو PNG).
- الـ API يرجع **url** جاهز تستخدمه في:
  - **mainImage** في Create/Update Hotel
  - **photos[].url** في Add Room

### في Postman
- Method: **POST**
- URL: `https://localhost:5091/api/v1/Upload/hotel-image` أو `.../room-image`
- **Body** → **form-data**
- Key: **file** | Type: **File** | Value: اختر الملف
- لا تضف **Content-Type** يدوياً؛ Postman يضبطه تلقائياً مع **multipart/form-data**.

### أين تُحفظ الصور؟
- عادةً في المشروع تحت مجلد مثل **wwwroot/uploads/** (مثلاً **hotels/** أو **rooms/**) حسب إعداد **IFileService**.
- الـ **url** في الـ response يكون مسار الوصول العام للصورة (مثلاً يستخدم في الـ API كـ mainImage أو room photo).

---

## 5) ملخص الـ Endpoints (Admin + Bookings)

| Method | المسار | الاستخدام |
|--------|--------|-----------|
| GET | /api/Dev/seed-data | جلب UserId, CategoryId, HotelId, RoomId للتجربة |
| POST | /api/v1/Upload/hotel-image | رفع صورة فندق (form-data, file) |
| POST | /api/v1/Upload/room-image | رفع صورة غرفة (form-data, file) |
| POST | /api/v1/admin/hotels | إنشاء فندق (JSON body) |
| PUT | /api/v1/admin/hotels/{id} | تحديث فندق (JSON body) |
| POST | /api/v1/admin/hotels/{hotelId}/rooms | إضافة غرفة (JSON body) |
| PUT | /api/v1/admin/rooms/{id} | تعديل غرفة (JSON body) |
| PUT | /api/v1/admin/rooms/{id}/availability | توفر الغرفة (JSON: startDate, endDate, isAvailable) |
| POST | /api/v1/admin/rooms/{id}/seasonal-pricing | سعر موسمي (JSON: startDate, endDate, price) |
| POST | /api/v1/bookings | إنشاء حجز (JSON: userId, categoryId, itemId, stayRange) |
| GET | /api/v1/bookings/{id} | عرض حجز بالـ Id |

---

## 6) أخطاء شائعة وحلولها

- **404 Not Found**  
  تأكد من الـ base URL (مثلاً https://localhost:5091) وأن المشروع شغال.

- **400 Bad Request / Validation**  
  - تأكد أن الـ JSON صحيح (أقواس، فواصل، أسماء الخصائص كما في الأمثلة).
  - **TimeOnly**: استخدم "14:00" أو "14:00:00".
  - **DateOnly**: استخدم "2026-03-01".
  - **DateTime** (stayRange): استخدم "2026-03-01T00:00:00" أو "2026-03-01".

- **Hotel not found / Room not found**  
  استخدم **HotelId** و **RoomId** من الـ responses أو من **GET /api/Dev/seed-data**.

- **User not found (عند الحجز)**  
  استخدم **userId** من **seed-data** (مثلاً المستخدم اللي emailه customer@onlinetravel.com).

- **No booking strategy for category**  
  تأكد أن **categoryId** يخص فئة من نوع **Hotel** (من **seed-data**).

بهذا تكون جربت كل endpoints الخاصة بـ Admin Hotels و Bookings من Swagger و Postman، مع رفع الصور وإدخال البيانات بالتفصيل.

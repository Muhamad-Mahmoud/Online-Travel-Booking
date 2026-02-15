# تجربة Hotel و Room فقط (بدون Bookings)

## تشغيل المشروع
```bash
cd OnlineTravelBookingTeamB
dotnet run
```
الرابط الافتراضي: **https://localhost:5091**

---

## 1) Swagger

افتح: **https://localhost:5091** (أو /swagger حسب الإعداد).

### ترتيب التجربة

| # | Method | المسار | ماذا تفعل |
|---|--------|--------|-----------|
| 1 | GET | `/api/v1/hotels` | بحث فنادق (اختياري: city, pageNumber, pageSize في Query) |
| 2 | POST | `/api/v1/Upload/hotel-image` | رفع صورة → Body: **form-data**، key **file**، نوع File |
| 3 | POST | `/api/v1/admin/hotels` | إنشاء فندق → Body: **raw JSON** (انظر المثال تحت) |
| 4 | GET | `/api/v1/hotels/{id}` | تفاصيل الفندق (ضع الـ id من الخطوة 3) |
| 5 | PUT | `/api/v1/admin/hotels/{id}` | تحديث الفندق → Body: raw JSON |
| 6 | POST | `/api/v1/Upload/room-image` | رفع صورة غرفة (form-data, file) |
| 7 | POST | `/api/v1/admin/hotels/{hotelId}/rooms` | إضافة غرفة → Body: raw JSON |
| 8 | GET | `/api/v1/hotels/{id}/rooms` | غرف الفندق (اختياري: checkin, checkout في Query) |
| 9 | PUT | `/api/v1/admin/rooms/{id}` | تعديل غرفة → Body: raw JSON |
| 10 | PUT | `/api/v1/admin/rooms/{id}/availability` | توفر الغرفة → Body: raw JSON |
| 11 | POST | `/api/v1/admin/rooms/{id}/seasonal-pricing` | سعر موسمي → Body: raw JSON |
| 12 | POST | `/api/v1/hotels/{id}/reviews` | إضافة تقييم → Body: raw JSON (userId, rating 1–5, comment) |

---

## 2) أمثلة الـ Body (JSON)

### إنشاء فندق — POST /api/v1/admin/hotels
```json
{
  "name": "فندق التجربة",
  "slug": "test-hotel-cairo",
  "description": "فندق في القاهرة.",
  "latitude": 30.0444,
  "longitude": 31.2357,
  "street": "شارع 123",
  "city": "Cairo",
  "state": "Cairo",
  "country": "Egypt",
  "postalCode": "11511",
  "mainImage": "https://example.com/hotel.jpg",
  "gallery": [],
  "checkInTime": "14:00",
  "checkOutTime": "11:00",
  "cancellationPolicy": "إلغاء مجاني 24 ساعة.",
  "contactPhone": "+201234567890",
  "contactEmail": "info@hotel.com",
  "website": "https://hotel.com"
}
```

### تحديث فندق — PUT /api/v1/admin/hotels/{id}
نفس الحقول (بدون slug لو الـ API لا يقبلها). **لا تضف id في الـ body**؛ الـ id من الـ URL.

### إضافة غرفة — POST /api/v1/admin/hotels/{hotelId}/rooms
```json
{
  "hotelId": "ضع-hotelId-هنا",
  "roomNumber": "101",
  "name": "غرفة مزدوجة",
  "description": "غرفة مريحة.",
  "basePricePerNight": 99.99,
  "photos": []
}
```
(يمكن وضع روابط صور من رفع room-image في `photos`: `[{ "url": "...", "alt": "غرفة 101" }]`)

### تعديل غرفة — PUT /api/v1/admin/rooms/{id}
```json
{
  "id": "نفس-roomId",
  "name": "غرفة مزدوجة (محدّثة)",
  "description": "وصف محدّث.",
  "basePricePerNight": 119.99
}
```

### توفر الغرفة — PUT /api/v1/admin/rooms/{id}/availability
```json
{
  "roomId": "نفس-roomId",
  "startDate": "2026-03-01",
  "endDate": "2026-03-31",
  "isAvailable": true
}
```

### سعر موسمي — POST /api/v1/admin/rooms/{id}/seasonal-pricing
```json
{
  "roomId": "نفس-roomId",
  "startDate": "2026-06-01",
  "endDate": "2026-08-31",
  "price": 149.99
}
```

### إضافة تقييم — POST /api/v1/hotels/{id}/reviews
تحتاج **userId** (GUID مستخدم موجود). يمكن الحصول عليه من **GET** `/api/Dev/seed-data` → `customer.id`.
```json
{
  "hotelId": "نفس-hotelId",
  "userId": "GUID-المستخدم",
  "rating": 5,
  "comment": "إقامة ممتازة"
}
```

---

## 3) Postman (Hotel و Room فقط)

- استورد الملف: **Postman_Collection_Hotel_And_Room_Only.json** (من جذر المشروع).
- غيّر المتغير **baseUrl** إذا البورت مختلف (مثلاً `https://localhost:5091`).
- بعد **Create Hotel** انسخ **id** → ضعه في **hotelId**.
- بعد **Add Room** انسخ **id** → ضعه في **roomId**.
- نفّذ الطلبات بالترتيب من 1 إلى 12 (بدون أي طلبات Bookings).

---

## ملخص الـ Endpoints (Hotel و Room فقط)

| Method | المسار |
|--------|--------|
| GET | /api/v1/hotels |
| GET | /api/v1/hotels/{id} |
| GET | /api/v1/hotels/{id}/rooms |
| POST | /api/v1/hotels/{id}/reviews |
| POST | /api/v1/Upload/hotel-image |
| POST | /api/v1/Upload/room-image |
| POST | /api/v1/admin/hotels |
| PUT | /api/v1/admin/hotels/{id} |
| POST | /api/v1/admin/hotels/{hotelId}/rooms |
| PUT | /api/v1/admin/rooms/{id} |
| PUT | /api/v1/admin/rooms/{id}/availability |
| POST | /api/v1/admin/rooms/{id}/seasonal-pricing |

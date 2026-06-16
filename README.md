# Product API

API ASP.NET Core dùng để thêm sản phẩm và lấy thông tin sản phẩm theo ID.
Dữ liệu hiện được lưu trong bộ nhớ và sẽ mất khi dừng ứng dụng.

## Chạy API

Tại thư mục solution, chạy lệnh:

```powershell
dotnet run --project Ex-Web-Core-API
```

Địa chỉ mặc định:

- HTTP: `http://localhost:5282`
- HTTPS: `https://localhost:7230`
- Đổi port tại: `.\Properties\launchSettings.json`

API không có giao diện tại đường dẫn `/`. Front-end cần gọi đúng các endpoint
được mô tả bên dưới.

## Các endpoint

### Thêm sản phẩm

```http
POST /api/products
Content-Type: application/json
```

Body:

```json
{
  "name": "Bàn phím cơ",
  "price": 1250000
}
```

Kết quả thành công: `201 Created`.

Quy tắc kiểm tra dữ liệu:

- `name` là bắt buộc và có ít nhất 3 ký tự.
- `price` phải lớn hơn 0.
- Dữ liệu không hợp lệ trả về `400 Bad Request` kèm danh sách lỗi.

### Lấy sản phẩm theo ID

```http
GET /api/products/{id}
```

Ví dụ:

```http
GET http://localhost:5282/api/products/1
```

- Thành công: `200 OK`.
- ID không phải số nguyên dương: `400 Bad Request`.
- Không tìm thấy sản phẩm: `404 Not Found`.

## Kết nối từ web

Ví dụ JavaScript dùng `fetch` để thêm sản phẩm:

```javascript
async function createProduct() {
  const response = await fetch("http://localhost:5282/api/products", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      name: "Bàn phím cơ",
      price: 1250000
    })
  });

  const data = await response.json();

  if (!response.ok) {
    console.error("Lỗi:", data);
    return;
  }

  console.log("Sản phẩm vừa tạo:", data);
}
```

Ví dụ lấy sản phẩm:

```javascript
async function getProduct(id) {
  const response = await fetch(
    `http://localhost:5282/api/products/${id}`
  );
  const data = await response.json();

  console.log(data);
}
```

Nếu website chạy ở domain hoặc cổng khác API, trình duyệt có thể chặn request
do chính sách CORS. Khi đó cần cấu hình CORS trong `Program.cs` và chỉ cho phép
origin của front-end, ví dụ `http://localhost:3000`.

## Kết nối từ Flutter

Cài package HTTP:

```powershell
flutter pub add http
```

Ví dụ gửi request:

```dart
import 'dart:convert';
import 'package:http/http.dart' as http;

Future<void> createProduct() async {
  final response = await http.post(
    Uri.parse('http://10.0.2.2:5282/api/products'),
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({
      'name': 'Bàn phím cơ',
      'price': 1250000,
    }),
  );

  final data = jsonDecode(response.body);
  print(data);
}
```

Địa chỉ API phụ thuộc vào nơi ứng dụng mobile đang chạy:

- Android Emulator: dùng `http://10.0.2.2:5282`.
- iOS Simulator: thường có thể dùng `http://localhost:5282`.
- Thiết bị thật: dùng IP LAN của máy chạy API, ví dụ
  `http://192.168.1.10:5282`.

Máy tính và thiết bị thật phải kết nối cùng mạng Wi-Fi. Firewall cũng phải cho
phép kết nối đến cổng API.

## Lưu ý khi phát triển

- Với Postman trên cùng máy, dùng `http://localhost:5282`.
- Request POST phải có header `Content-Type: application/json`.
- Nên dùng HTTP khi thử nghiệm local để tránh lỗi chứng chỉ HTTPS trên mobile.
- Phải tạo sản phẩm bằng POST trước khi gọi GET theo ID.

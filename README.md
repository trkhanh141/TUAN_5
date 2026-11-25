# OFFSTEP — ASP.NET Core MVC (.NET 10)

Website bán giày thời trang theo phong cách tối giản đen–trắng–xanh lime. Dự án được tổ chức theo yêu cầu bài tập Phát triển ứng dụng TMĐT: MVC, SQL Server/EF Core, CRUD sản phẩm và thể loại, upload ảnh, giỏ hàng Session và Checkout lưu đơn hàng.

## Công nghệ
- .NET 10 / ASP.NET Core MVC / C#
- Entity Framework Core 10 + SQL Server
- ASP.NET Core Identity (đăng ký, đăng nhập, phân quyền Admin)
- Session để lưu giỏ hàng; Razor Views; CSS responsive

## Chức năng
- Cửa hàng: tìm kiếm, lọc theo thể loại, xem chi tiết.
- Quản trị (role `Admin`): CRUD sản phẩm, CRUD thể loại; upload ảnh JPG/JPEG/PNG/WebP tối đa 5 MB; xóa sản phẩm theo cơ chế ẩn.
- Giỏ hàng: thêm, cập nhật số lượng, xóa, tính tổng tiền.
- Checkout: xác thực thông tin nhận hàng, kiểm tra tồn kho, lưu đơn hàng và chi tiết vào SQL Server, trừ tồn kho, hiển thị trang hoàn tất.
- Seed dữ liệu demo lần đầu: 3 thể loại và 6 sản phẩm. Có thể chỉnh sửa hoặc thay thế trong `Data/SeedData.cs`.

## Yêu cầu môi trường
- .NET 10 SDK
- SQL Server (LocalDB/SQL Server Express/SQL Server trên Somee)
- VS Code + C# Dev Kit hoặc Visual Studio

## Cấu hình database
Không commit connection string có username/password. Tạo user-secrets cho máy local:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER;Database=OFFSTEP;User Id=YOUR_LOGIN;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
dotnet user-secrets set "Admin:Email" "email-ban-se-dung-de-dang-ky@example.com"
dotnet user-secrets set "Email:Address" "your-gmail@gmail.com"
dotnet user-secrets set "Email:AppPassword" "your-16-character-app-password"
```

Có thể tham khảo `appsettings.example.json`; không điền thông tin thật vào file mẫu. Với Somee, dùng đúng connection string trong Dashboard và thêm `TrustServerCertificate=True` ở cuối.

## Cài đặt và chạy
Tại thư mục `SourceCode`:

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 10.0.*
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

Mở URL HTTPS được in ở dòng `Now listening on`. Lần đầu chạy sau khi database đã tạo, ứng dụng tự thêm role `Admin`, 3 thể loại và 6 sản phẩm mẫu.

### Tạo tài khoản quản trị
1. Đặt `Admin:Email` trong user-secrets trùng với email bạn định đăng ký.
2. Chạy website, đăng ký tài khoản bằng email đó.
3. Khởi động lại ứng dụng. Startup sẽ gán role `Admin` cho tài khoản đã đăng ký.
4. Mở `/AdminProducts` hoặc chọn Quản trị trên thanh menu.

Tài khoản người dùng thông thường không có quyền truy cập khu vực quản trị.

## Cấu trúc
```
ASP-CORE/
  README.md
  SourceCode/
    global.json
    OFFSTEP.csproj
    Controllers/       # Store, Cart, Checkout, Admin
    Data/              # DbContext, seed data
    Models/            # Product, Category, Order, OrderItem
    Services/          # CartService
    ViewModels/
    Views/             # Razor Views
    wwwroot/           # CSS, JS, ảnh mặc định, ảnh upload
    Migrations/        # EF Core migrations (generate on first setup)
    Database/          # CreateDatabase.sql, SampleData.sql
  Report/
    BaiTap_CaNhan_ASP_CORE_LMS.docx
```

## Publish lên Somee
Sau khi xác nhận gói hosting hỗ trợ runtime .NET 10 và đã cấu hình connection string an toàn:

```bash
dotnet publish -c Release -o ./publish
```

Nén **nội dung bên trong** thư mục `publish` thành ZIP (để `web.config` nằm ở gốc ZIP), sau đó upload/unzip qua File Manager Somee. Không commit thư mục `publish` hoặc file cấu hình chứa mật khẩu. Hosting chưa được cấu hình trong bộ mã nguồn này; chưa có URL triển khai.

## Lưu ý
- Không có ảnh minh chứng thực tế trong gói này theo yêu cầu người dùng. Cần tự chạy, kiểm thử và chụp ảnh nếu giảng viên/LMS yêu cầu.
- Email xác nhận đơn hàng là tùy chọn: cấu hình `Email:Address` và `Email:AppPassword` bằng user-secrets. Không dùng mật khẩu Gmail thông thường. Nếu SMTP lỗi/bị hosting chặn, đơn hàng vẫn được lưu và trang hoàn tất vẫn hiển thị.
- Migrations chưa được sinh sẵn trong ZIP; hãy chạy lệnh `dotnet ef migrations add InitialCreate` sau khi cấu hình connection string. Đây là bước bắt buộc trước lần chạy database đầu tiên. `global.json` ghim SDK .NET 10 (roll forward trong nhánh feature) theo hướng dẫn học phần.
- Nên dùng dữ liệu và ảnh sản phẩm do bạn tự chọn/thay thế trước khi nộp chính thức.

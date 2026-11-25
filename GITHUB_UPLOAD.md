# Đưa thư mục ASP-CORE lên repository TUAN_5

Bộ ZIP này chứa sẵn thư mục `ASP-CORE`. Để không ghi đè các bài thực hành cũ:

```bash
git clone https://github.com/trkhanh141/TUAN_5.git
cd TUAN_5
```

Giải nén ZIP vào thư mục repository vừa clone. Kiểm tra cấu trúc phải là `TUAN_5/ASP-CORE/README.md` (không phải `TUAN_5/ASP-CORE/ASP-CORE/README.md`). Sau đó:

```bash
git status
git add ASP-CORE
git commit -m "Add OFFSTEP ASP.NET Core MVC assignment"
git push
```

Nếu repository có nhánh mặc định khác `main`, thao tác trên nhánh hiện tại. Trước khi push, kiểm tra `git status` và bảo đảm không có connection string/mật khẩu thật trong thay đổi. Không xóa hoặc di chuyển các thư mục bài cũ.

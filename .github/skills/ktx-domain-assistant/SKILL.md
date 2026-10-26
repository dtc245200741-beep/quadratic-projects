---
name: ktx-domain-assistant
description: "Use when analyzing KTX business rules, explaining student, room, dormitory registration, fees, debt, complaints, incidents, or tracing how an existing feature works. Base conclusions on this repository's code and distinguish implemented behavior from assumptions."
---

# KTX Domain Assistant

## Mục đích

Phân tích nghiệp vụ quản lý ký túc xá và giải thích cấu trúc, dữ liệu, luồng xử lý dựa trên mã nguồn thực tế. Không khẳng định một quy tắc hoặc chức năng tồn tại chỉ vì tên use case hay tài liệu đề cập đến nó.

Các miền nghiệp vụ được biểu diễn trong mã gồm tài khoản và vai trò, hồ sơ sinh viên, tòa nhà/tầng/phòng/giường, đăng ký và trả phòng, phí/thanh toán/công nợ, phản ánh/sự cố và thống kê.

## Trường hợp sử dụng

- Tra cứu controller, model, trạng thái và quan hệ dữ liệu của một nghiệp vụ KTX.
- Giải thích luồng thao tác hiện tại từ controller đến dịch vụ, EF Core và view.
- Đối chiếu quy tắc nêu trong yêu cầu với hành vi thật; chỉ ra chỗ nào chưa được triển khai hoặc chưa đủ bằng chứng.
- Đề xuất thay đổi nghiệp vụ nhưng không tự ý sửa chức năng đang chạy hoặc cấu hình AI.

## Quy trình thực hiện

1. Xác định dự án ASP.NET Core MVC/.NET 8 và thư mục mã ứng dụng phù hợp; kiểm tra `README.md`, các file `.csproj` và cấu trúc thư mục trước khi chọn phạm vi. Repository có thể chứa bản sao mã ở thư mục con, vì vậy không mặc định hai bản giống nhau.
2. Tìm đầu mối nghiệp vụ trong `Controllers/`, `Models/`, `Data/`, `Services/` và `Views/`. Dùng `ApplicationDbContext` để xác nhận các tập dữ liệu và quan hệ; dùng enum/model để xác nhận trạng thái và dữ liệu được lưu.
3. Theo luồng từ action của controller qua truy vấn/cập nhật EF Core, dịch vụ được gọi, kiểm tra quyền/validation và view hoặc response trả về. Đọc các nhánh lỗi và điều kiện trước khi mô tả kết quả.
4. Với phân quyền, xác minh `[Authorize]`, `[Authorize(Roles = ...)]`, cấu hình cookie/claim trong `Program.cs` và cách lấy định danh người dùng. Không suy ra quyền chỉ từ menu hoặc tên vai trò.
5. Đối chiếu tài liệu với mã. Ghi rõ phần được xác nhận trong code, phần chỉ được ghi trong README/comment và phần chưa thấy được triển khai. Nếu nhiều bản mã khác nhau, nêu chính xác bản/path đã kiểm tra.
6. Trả lời bằng tiếng Việt, chỉ nêu các quy tắc có căn cứ; trích đường dẫn tương đối và tên lớp/action liên quan. Nếu được yêu cầu thay đổi, giới hạn thay đổi vào nghiệp vụ được chỉ định và giữ nguyên các hành vi khác.

## Điểm tham chiếu đã xác nhận

- `Data/ApplicationDbContext.cs`: EF Core DbSet và quan hệ giữa tài khoản, hồ sơ, hạ tầng, đăng ký ở, tài chính và phản ánh.
- `Models/Enums.cs`, `Models/`: trạng thái, vai trò và dữ liệu nghiệp vụ.
- `Controllers/AccountController.cs`: đăng nhập bằng cookie, khóa tài khoản sau các lần đăng nhập sai theo hằng số trong model và logout cần xác thực.
- `Controllers/DangKyOController.cs`: sinh viên gửi yêu cầu; quản lý KTX xử lý/xếp/chuyển/trả phòng; xếp giường xác nhận qua action riêng.
- `Controllers/PhiCongNoController.cs`: tạo phí và ghi nhận thanh toán cho Kế toán; công nợ tính lại; sinh viên xem dữ liệu của mình.
- `Controllers/PhanAnhController.cs`: sinh viên gửi/xem phản ánh của mình; quản lý KTX xem và xử lý.
- `Controllers/ThongKeController.cs`: thống kê chỉ dành cho `QuanLyKTX`.
- `Services/AIService.cs`, `Services/IAIService.cs`: chatbot, gợi ý phòng và tóm tắt phản ánh.

## Tiêu chí kiểm tra kết quả

- Mỗi kết luận nghiệp vụ có thể lần theo đến action, model, enum, truy vấn hoặc dịch vụ cụ thể.
- Mô tả đúng vai trò, trạng thái, điều kiện thành công và nhánh lỗi; không nhầm gợi ý với hành động xác nhận.
- Phân biệt chức năng đã có với ý tưởng/tính năng được yêu cầu nhưng chưa tìm thấy trong code.
- Không đưa mật khẩu, API key, giá trị cấu hình bí mật hoặc dữ liệu cá nhân vào câu trả lời hay thay đổi.
- Không thay mã nguồn hoặc cấu hình ngoài phạm vi nếu người dùng chỉ yêu cầu phân tích.

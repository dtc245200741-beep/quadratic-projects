---
name: ktx-test-and-validate
description: "Use when validating KTX MVC changes, planning or running tests, checking model input, role authorization, antiforgery, error handling, or explaining how to restore, build, and run this ASP.NET Core .NET 8 repository."
---

# KTX Test and Validate

## Mục đích

Hướng dẫn kiểm tra thay đổi trong ứng dụng ASP.NET Core MVC .NET 8 theo hành vi và cấu trúc thực tế. Tập trung vào đầu vào, phân quyền, lỗi, luồng dữ liệu và xác minh lệnh build/test/run; không tự sửa code hoặc cấu hình kết nối AI khi chỉ được yêu cầu kiểm thử.

## Trường hợp sử dụng

- Lập checklist hoặc kiểm thử thay đổi ở controller, model, service hay view.
- Viết test case có tiền điều kiện, dữ liệu/role, bước thực hiện và kết quả mong đợi.
- Rà validation, quyền theo vai trò, dữ liệu người dùng hiện tại, antiforgery và các nhánh lỗi.
- Hướng dẫn restore/build/test/run ứng dụng bằng .NET 8.
- Xác nhận giới hạn của môi trường kiểm thử, bao gồm EF Core InMemory và việc không tìm thấy test project.

## Quy trình thực hiện

1. Xem `README.md`, các `.csproj`/solution, cấu trúc thư mục và `git status` trước khi chạy lệnh. Chọn đúng project được tài liệu và cấu trúc ứng dụng chỉ ra; repository có thể chứa project hoặc bản sao mã ở cả thư mục gốc và thư mục con.
2. Xác định phạm vi thay đổi và lần theo action, model/viewmodel, dịch vụ, DbContext, view và các caller liên quan. Đọc các nhánh thành công, lỗi và cập nhật dữ liệu; không dựa riêng vào tên test hay tài liệu.
3. Kiểm tra dữ liệu đầu vào: DataAnnotations/ModelState, trường bắt buộc, dữ liệu rỗng, giá trị số/enum, ID không tồn tại, giới hạn nghiệp vụ và thông báo lỗi. Kiểm tra controller có trả view/model phù hợp khi validation thất bại.
4. Kiểm tra phân quyền dựa trên `[Authorize]` và `Roles`, cookie/claim trong `Program.cs`, cùng việc truy vấn dữ liệu theo tài khoản đang đăng nhập. Thử ma trận người chưa đăng nhập, vai trò hợp lệ, vai trò không hợp lệ và dữ liệu thuộc người dùng khác khi có liên quan.
5. Kiểm tra các POST thay đổi dữ liệu có `[ValidateAntiForgeryToken]` theo pattern hiện có; kiểm tra không tìm thấy bản ghi, trạng thái không hợp lệ, thao tác lặp/cạnh tranh và các phản hồi `NotFound`, redirect hoặc lỗi theo thiết kế.
6. Khi được yêu cầu viết test case, trình bày từng ca với mã/tên, mục tiêu, tiền điều kiện, vai trò, dữ liệu đầu vào, bước thực hiện và kết quả mong đợi. Bao gồm đường đi thành công cùng các trường hợp không hợp lệ, thiếu quyền, không tìm thấy bản ghi và lỗi; chỉ thêm các trường hợp phù hợp với hành vi thực tế. Có thể dùng bảng Markdown nếu người dùng không yêu cầu định dạng khác.
7. Trước khi khẳng định có unit/integration test, tìm test project và test hiện hữu. Nếu không có, báo rõ; không tạo test framework hoặc sửa ứng dụng trừ khi được yêu cầu. Chạy test phù hợp nếu có.
8. Dùng SDK .NET 8 trở lên đã cài. Theo README, ứng dụng có thể chạy từ thư mục project chứa `KTXManagement.Demo.csproj` bằng `dotnet restore`, `dotnet build` và `dotnet run`; xác minh project path thực tế trước khi dùng `--project`. Chờ ứng dụng khởi động, mở URL được in ở console và kiểm thử luồng cần thiết.
9. Ghi lại chính xác lệnh đã chạy, kết quả, lỗi hoặc cảnh báo và phần chưa kiểm thử. Không báo build/test thành công nếu lệnh thất bại hoặc chưa chạy.

## Lưu ý đặc thù repository

- `Program.cs` đăng ký MVC, `ApplicationDbContext` với EF Core InMemory, cookie authentication, authorization, antiforgery header và `IAIService`.
- Vai trò được dùng trong code gồm `QuanLyKTX`, `SinhVien`, `KeToan`. Controller phân quyền theo vai trò; ví dụ quản lý hạ tầng/thống kê thuộc Quản lý KTX, thanh toán thuộc Kế toán, một số trang cá nhân thuộc Sinh viên.
- `SeedData.Initialize` chạy khi khởi động. Dữ liệu EF Core InMemory không bền sau khi tiến trình dừng; không dùng tính chất này để kết luận trạng thái dữ liệu production.
- `ChatController` yêu cầu đăng nhập và POST Ask có antiforgery; đừng đưa API key hay giá trị bí mật vào log, lệnh, ảnh chụp hoặc kết quả test.
- Không thấy test project khi kiểm tra repository; hãy tìm lại trước mỗi lần đánh giá vì cấu trúc có thể đã thay đổi.

## Tiêu chí kiểm tra kết quả

- Đã xác minh đúng project/entry point và phạm vi thay đổi, không bỏ qua bản mã liên quan.
- Các trường hợp hợp lệ, không hợp lệ, thiếu quyền, không tìm thấy dữ liệu và lỗi đều được xem xét hoặc ghi rõ chưa kiểm thử.
- Quyền được xác minh tại server-side controller, không chỉ dựa vào giao diện.
- Lệnh restore/build/test/run và kết quả được báo chính xác; không tuyên bố có test khi repository không có hoặc chưa chạy.
- Không thay đổi file ngoài phạm vi kiểm thử, không xóa file và không tiết lộ/sửa cấu hình AI khi chưa được yêu cầu.

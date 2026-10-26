---
name: ktx-ai-feature-builder
description: "Use when analyzing, designing, implementing, or changing KTX AI features such as the existing chatbot, room recommendations, incident summaries, or a proposed incident classifier. Inspect the repository's current AI provider, service interface, configuration, and fallback behavior before proposing changes."
---

# KTX AI Feature Builder

## Mục đích

Hỗ trợ thiết kế và phát triển tính năng AI phù hợp với kiến trúc KTX đang có. Trước khi đề xuất công nghệ hoặc chỉnh sửa, phải xác minh cách AI hiện được gọi, ranh giới nghiệp vụ và hành vi dự phòng trong source hiện tại. Giữ nguyên cấu hình AI nếu người dùng không yêu cầu thay đổi.

## Trường hợp sử dụng

- Tìm hiểu hoặc chỉnh chatbot hỏi-đáp.
- Tìm hiểu hoặc cải tiến gợi ý xếp phòng và tóm tắt phản ánh dùng trong thống kê.
- Đánh giá yêu cầu mới như phân loại phản ánh, xác định rõ đây là yêu cầu đề xuất hay chức năng đã triển khai.
- Kiểm tra luồng lỗi, quyền truy cập và dữ liệu đầu vào/đầu ra của một tích hợp AI.

## Công nghệ và hành vi hiện có đã xác nhận

- Đăng ký `IAIService`/`AIService` trong `Program.cs`; interface có `TraLoiAsync`, `GoiYPhongAsync` và `TomTatPhanAnhAsync`.
- `AIService` dùng `IHttpClientFactory`, `IConfiguration`, `ILogger` và `System.Text.Json` để gửi HTTP JSON tới API tương thích Anthropic. Mặc định endpoint/model nằm trong service; cấu hình đọc từ `AIConfig:ApiKey`, `AIConfig:Model` và `AIConfig:ApiUrl`.
- Nếu không có API key, service sử dụng câu trả lời rule-based; chatbot cũng có các câu trả lời cá nhân lấy từ dữ liệu sinh viên. Lỗi hoặc phản hồi không thành công từ API được ghi log warning và chuyển sang rule-based.
- Gợi ý xếp phòng được tính theo quy tắc hiện có: loại phòng bảo trì và phòng hết chỗ, ràng buộc giới tính, ưu tiên cùng lớp/khoa và số giường trống; kết quả là gợi ý để quản lý xem xét, không tự xác nhận xếp giường.
- Tóm tắt phản ánh được gọi trong thống kê; quyền xem thống kê trong controller hiện giới hạn `QuanLyKTX`.
- ChatController yêu cầu đăng nhập; endpoint hỏi đáp POST dùng antiforgery. Chức năng phân loại phản ánh độc lập chưa thấy trong `IAIService` hiện có; không mô tả nó như tính năng đã triển khai.

## Quy trình thực hiện

1. Đọc `README.md`, file dự án và `Program.cs`; xác định chính xác project đang chạy và các bản mã liên quan trước khi thay đổi.
2. Đọc `Services/IAIService.cs`, toàn bộ phương thức liên quan trong `Services/AIService.cs`, controller/view gọi phương thức đó, model/viewmodel và cấu hình tham chiếu. Tìm các caller bằng tìm kiếm symbol/text thay vì suy đoán.
3. Ghi lại hiện trạng: provider/protocol, config keys (chỉ tên, không giá trị bí mật), model/endpoint mặc định nếu liên quan, serialization, timeout, logging, fallback, dữ liệu đưa vào prompt và quyền của caller. Không yêu cầu hoặc hiển thị API key.
4. So sánh yêu cầu với hiện trạng. Phân biệt rõ: đã có, chưa có, hoặc cần xác nhận; không tự thay provider, model, URL, API key, prompt nền hay phương thức fallback.
5. Trước khi triển khai, xác định ranh giới nghiệp vụ và dữ liệu tối thiểu cần gửi cho dịch vụ bên ngoài. Tránh gửi định danh cá nhân hoặc dữ liệu nhạy cảm không cần thiết; giữ quy tắc nghiệp vụ có tính ràng buộc ở phía ứng dụng.
6. Nếu được yêu cầu sửa code, dùng interface/service hiện có khi phù hợp; cập nhật mọi caller liên quan, xử lý kết quả rỗng/lỗi minh bạch theo pattern của repository, giữ fallback đã có và không để AI tự thực hiện thao tác ghi/xếp phòng thay người có quyền.
7. Kiểm tra quyền controller, validation/anti-forgery cho endpoint, log không chứa bí mật, nhánh không có API key và nhánh lỗi nhà cung cấp. Chạy build/test thích hợp của project thực tế; nếu không có test project thì nói rõ và không tuyên bố test đã chạy.

## Tiêu chí kiểm tra kết quả

- Đề xuất dựa trên `IAIService`/`AIService` và caller hiện tại; công nghệ mới chỉ được giới thiệu khi yêu cầu có căn cứ và người dùng chấp thuận.
- Không thay đổi cấu hình kết nối AI hoặc ghi giá trị bí mật nếu chưa được yêu cầu.
- Kết quả AI không được thay thế xác nhận nghiệp vụ của quản lý/Kế toán hoặc kiểm tra dữ liệu có thẩm quyền.
- Tính năng có đường đi rõ ràng cho không có cấu hình API, lỗi provider, đầu ra rỗng và dữ liệu không hợp lệ.
- Các quyền truy cập, antiforgery và phạm vi dữ liệu đúng với controller hiện hữu.
- Phân biệt được phần hiện hữu với phần mới; build/test chỉ được báo thành công khi đã thực sự chạy và thành công.

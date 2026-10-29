using KTXManagement.Demo.Data;
using KTXManagement.Demo.Models.ViewModels;
using KTXManagement.Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KTXManagement.Demo.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IAIService _aiService;

        public ChatController(ApplicationDbContext db, IAIService aiService)
        {
            _db = db;
            _aiService = aiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ask([FromBody] ChatRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new ChatResponse
                {
                    Reply = "Yeu cau khong hop le."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new ChatResponse
                {
                    Reply = "Vui long nhap cau hoi."
                });
            }

            if (request.Message.Length > ChatRequest.MaxMessageLength)
            {
                return BadRequest(new ChatResponse
                {
                    Reply = $"Cau hoi khong duoc vuot qua {ChatRequest.MaxMessageLength} ky tu."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ChatResponse
                {
                    Reply = "Du lieu cau hoi khong hop le."
                });
            }

            var taiKhoanId = User.GetTaiKhoanId();
            var taiKhoan = await _db.TaiKhoans.FindAsync(taiKhoanId);
            var traLoi = await _aiService.TraLoiAsync(request.Message, taiKhoan);

            return Json(new ChatResponse { Reply = traLoi });
        }
    }
}

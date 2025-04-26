using Microsoft.AspNetCore.Mvc;

namespace CmsTHTN.WebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(string name, string email, string message)
        {
            // ✅ Xử lý logic gửi tin nhắn
            Console.WriteLine($"Gửi tin nhắn từ: {name}, Email: {email}, Nội dung: {message}");

            TempData["SuccessMessage"] = "Tin nhắn của bạn đã được gửi thành công!";
            return RedirectToAction("Index");
        }
    }
}

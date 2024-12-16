using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebBanHang.Models;
using System.Linq;

namespace WebBanHang.Controllers
{
    public class ThongTinNguoiDungController : Controller
    {
        private readonly FashionShopDbContext _context;

        public ThongTinNguoiDungController(FashionShopDbContext context)
        {
            _context = context;
        }

        // Action để hiển thị thông tin người dùng
        public IActionResult Index()
        {
            // Lấy ID người dùng từ Session hoặc Claims
            var userIdFromSession = HttpContext.Session.GetString("UserID");
            var userIdFromClaims = User.Claims.FirstOrDefault(c => c.Type == "ID")?.Value;

            // Nếu ID từ Session không có, dùng ID từ Claims
            var userId = !string.IsNullOrEmpty(userIdFromSession) ? userIdFromSession : userIdFromClaims;

            if (string.IsNullOrEmpty(userId))
            {
                // Nếu không có ID người dùng, điều hướng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }

            // Chuyển ID sang kiểu int
            var userIdInt = int.Parse(userId);

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = _context.NguoiDung.SingleOrDefault(u => u.ID == userIdInt);

            if (nguoiDung == null)
            {
                // Nếu không tìm thấy người dùng, điều hướng về trang đăng nhập
                return RedirectToAction("Login", "Home");
            }

            // Trả về view với thông tin người dùng
            return View(nguoiDung);
        }
    }
}

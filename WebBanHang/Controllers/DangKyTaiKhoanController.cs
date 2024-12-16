using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using BC = BCrypt.Net.BCrypt;
using System.Threading.Tasks;

namespace WebBanHang.Controllers
{
    public class DangKyTaiKhoanController : Controller
    {
        private readonly FashionShopDbContext _context;

        // Constructor để inject FashionShopDbContext vào controller
        public DangKyTaiKhoanController(FashionShopDbContext context)
        {
            _context = context;
        }

        // GET: DangKyTaiKhoan/Index
        public IActionResult Index()
        {
            return View();
        }

        // POST: DangKyTaiKhoan/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(NguoiDung model)
        {
            if (_context.NguoiDung.Any(u => u.TenDangNhap == model.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại!");
            }

            if (model.MatKhau != model.XacNhanMatKhau)
            {
                ModelState.AddModelError("XacNhanMatKhau", "Xác nhận mật khẩu không khớp!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mã hóa mật khẩu trước khi lưu
                    model.MatKhau = BC.HashPassword(model.MatKhau);

                    // Đặt quyền hạn mặc định là false
                    model.Quyen = false;

                    // Thêm tài khoản vào cơ sở dữ liệu
                    _context.NguoiDung.Add(model);
                    await _context.SaveChangesAsync();

                    // Chuyển hướng đến trang đăng ký thành công
                    return RedirectToAction("DangKyThanhCong");
                }
                catch (Exception ex)
                {
                    // Log lỗi và thêm thông báo lỗi
                    ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
                }
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form với lỗi
            return View(model);
        }

        // GET: DangKyTaiKhoan/DangKyThanhCong
        public IActionResult DangKyThanhCong()
        {
            return View();
        }
    }
}

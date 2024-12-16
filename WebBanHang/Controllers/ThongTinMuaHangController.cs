using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ThongTinMuaHangController : Controller
    {
        private readonly FashionShopDbContext _context;

        public ThongTinMuaHangController(FashionShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy UserID từ session
            var userIdSession = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdSession) || !int.TryParse(userIdSession, out int userId))
            {
                return Unauthorized("Người dùng chưa đăng nhập hoặc thông tin không hợp lệ.");
            }

            // Truy vấn lấy danh sách đặt hàng của người dùng
            var donDatHang = await _context.DatHang
                .Where(dh => dh.NguoiDungID == userId)
                .Include(dh => dh.NguoiDung)
                .Include(dh => dh.TinhTrang)
                .Include(dh => dh.DatHang_ChiTiet)
                    .ThenInclude(dhct => dhct.SanPham)
                .ToListAsync();

            if (!donDatHang.Any())
            {
                return View("KhongCoDonHang"); // Hiển thị View thông báo không có đơn hàng
            }

            // Không sử dụng ViewModel mà trả trực tiếp Model `DatHang`
            return View(donDatHang);
        }

        [HttpPost]
        public IActionResult HuyDonHang(int orderId)
        {
            // Tìm đơn hàng theo ID
            var order = _context.DatHang.FirstOrDefault(o => o.ID == orderId);

            if (order != null)
            {
                // Kiểm tra tình trạng đơn hàng (nếu tình trạng là "Đang vận chuyển", không cho phép hủy)
                if (order.TinhTrangID == 4)
                {
                    TempData["ErrorMessage"] = "Đơn hàng đang vận chuyển, không thể hủy!";
                    return RedirectToAction("Index"); // Quay lại trang danh sách
                }

                // Xóa đơn hàng nếu không phải "Đang vận chuyển"
                _context.DatHang.Remove(order);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đơn hàng đã được hủy thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Đơn hàng không tồn tại!";
            }

            return RedirectToAction("Index"); // Quay lại trang danh sách
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace WebBanHang.Controllers
{
    public class GioHangController : Controller
    {
        private readonly FashionShopDbContext _context;

        public GioHangController(FashionShopDbContext context)
        {
            _context = context;
        }

        // Phương thức xử lý thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult Them(int SanPhamID, string TenDangNhap, string KichCo, int SoLuongTrongGio, string NgayMua)
        {
            // Lấy thông tin sản phẩm từ database (chỉ lấy thông tin cần thiết, không lưu đơn giá vào cơ sở dữ liệu)
            var sanPham = _context.SanPham.FirstOrDefault(sp => sp.ID == SanPhamID);
            if (sanPham == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            // Thêm vào giỏ hàng
            var gioHang = new GioHang
            {
                SanPhamID = SanPhamID,
                TenDangNhap = TenDangNhap,
                KichCo = KichCo,
                SoLuongTrongGio = SoLuongTrongGio,
                ThoiGian = DateTime.Parse(NgayMua) // Lưu ngày mua vào CSDL
            };

            _context.GioHang.Add(gioHang);
            _context.SaveChanges();

            // Quay lại trang giỏ hàng hoặc trang xác nhận đơn hàng
            return RedirectToAction("Index", "GioHang");
        }



        // Action để hiển thị giỏ hàng
        public IActionResult Index()
        {
            // Lấy giỏ hàng của người dùng từ cơ sở dữ liệu
            var gioHang = _context.GioHang
                                  .Include(g => g.SanPham)  // Include SanPham để có thể truy cập thông tin sản phẩm
                                  .Where(g => g.TenDangNhap == User.Identity.Name)  // Lọc theo tên đăng nhập của người dùng
                                  .ToList();
<<<<<<< HEAD
<<<<<<< HEAD
            // Kiểm tra nếu giỏ hàng rỗng
            if (gioHang == null || !gioHang.Any())
            {
                // Chuyển hướng đến trang Giỏ hàng rỗng
                return View("GioHangRong");
            }
=======
>>>>>>> 71b5da4b29821cdcf62ed5021b9245bc3f2a3f69
=======
>>>>>>> 71b5da4b29821cdcf62ed5021b9245bc3f2a3f69

            return View(gioHang);
        }

        [HttpPost]
        public IActionResult XoaSanPham(int SanPhamID, string TenDangNhap)
        {
            // Lấy giỏ hàng của người dùng từ cơ sở dữ liệu
            var gioHang = _context.GioHang.FirstOrDefault(g => g.SanPhamID == SanPhamID && g.TenDangNhap == TenDangNhap);

            if (gioHang != null)
            {
                _context.GioHang.Remove(gioHang);
                _context.SaveChanges();
            }

            // Chuyển hướng lại đến giỏ hàng sau khi xóa
            return RedirectToAction("Index");
        }

    }
}

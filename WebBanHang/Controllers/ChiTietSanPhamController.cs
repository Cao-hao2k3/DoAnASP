using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        private readonly FashionShopDbContext _context;

        public ChiTietSanPhamController(FashionShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            // Lấy sản phẩm dựa vào ID
            var sanPham = _context.SanPham
                .Include(sp => sp.LoaiSanPham) // Nếu có quan hệ với loại sản phẩm
                .Include(sp => sp.HangSanXuat) // Nếu có quan hệ với hãng sản xuất
                .FirstOrDefault(sp => sp.ID == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }
}

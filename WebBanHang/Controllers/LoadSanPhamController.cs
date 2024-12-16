using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models; // Namespace chứa các Model và DbContext
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Controllers
{
    public class LoadSanPhamController : Controller
    {
        private readonly FashionShopDbContext _context;

        public LoadSanPhamController(FashionShopDbContext context)
        {
            _context = context;
        }

        // Action Index để hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var sanPhams = await _context.SanPham
                                         .Include(sp => sp.LoaiSanPham) // Include loại sản phẩm (nếu cần)
                                         .Include(sp => sp.HangSanXuat) // Include hãng sản xuất (nếu cần)
                                         .ToListAsync();
            return View(sanPhams);
        }

        // Action Details để hiển thị chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var sanPham = await _context.SanPham
                                        .Include(sp => sp.LoaiSanPham)
                                        .Include(sp => sp.HangSanXuat)
                                        .FirstOrDefaultAsync(sp => sp.ID == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
    }
}

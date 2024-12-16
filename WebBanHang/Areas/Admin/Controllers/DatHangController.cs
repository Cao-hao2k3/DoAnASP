using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DatHangController : Controller
    {
        private readonly FashionShopDbContext _context;

        public DatHangController(FashionShopDbContext context)
        {
            _context = context;
        }

        // GET: DatHang
        public async Task<IActionResult> Index()
        {
            var fashionShopDbContext = _context.DatHang.Include(d => d.NguoiDung).Include(d => d.TinhTrang);
            return View(await fashionShopDbContext.ToListAsync());
        }

        // POST: DatHang/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            var datHang = await _context.DatHang.FindAsync(id);
            if (datHang == null)
            {
                return NotFound("Không tìm thấy đơn hàng!");
            }

            // Cập nhật tình trạng
            datHang.TinhTrangID = status;

            try
            {
                _context.Update(datHang);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Không thể cập nhật tình trạng!");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

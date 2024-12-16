using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebBanHang.Models;
using System.Collections.Generic;

namespace WebBanHang.Controllers
{
    public class DatHangController : Controller
    {
        private readonly FashionShopDbContext _context;

        // Constructor để khởi tạo DbContext
        public DatHangController(FashionShopDbContext context)
        {
            _context = context;
        }
        // POST: Lấy thông tin sản phẩm từ giỏ hàng và xử lý đơn hàng
        [HttpPost]
        public IActionResult Index(List<int> SanPhamIDs, List<string> TenSanPhams, List<int> SoLuongs, List<string> KichCos, List<decimal> DonGias)
        {
            // Lấy ID người dùng từ claims
            string userId = User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ThongBaoLoi"] = "Bạn cần đăng nhập để thực hiện hành động này.";
                return RedirectToAction("Login", "Home");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = _context.NguoiDung.FirstOrDefault(u => u.ID.ToString() == userId);

            if (nguoiDung == null)
            {
                TempData["ThongBaoLoi"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Home");
            }

            // Truyền thông tin người dùng vào ViewBag
            ViewBag.UserID = nguoiDung.ID;
            ViewBag.HoVaTen = nguoiDung.HoVaTen;
            ViewBag.Email = nguoiDung.Email;
            ViewBag.DienThoai = nguoiDung.DienThoai;
            ViewBag.DiaChi = nguoiDung.DiaChi;

            // Xử lý các sản phẩm trong giỏ hàng
            var danhSachSanPham = new List<SanPhamViewModel>();
            for (int i = 0; i < SanPhamIDs.Count; i++)
            {
                // Kiểm tra các thông tin sản phẩm có hợp lệ hay không
                if (SoLuongs[i] <= 0 || DonGias[i] <= 0)
                {
                    TempData["ThongBaoLoi"] = "Thông tin sản phẩm không hợp lệ.";
                    return RedirectToAction("Index", "GioHang");
                }

                danhSachSanPham.Add(new SanPhamViewModel
                {
                    ID = SanPhamIDs[i],
                    TenSanPham = TenSanPhams[i],
                    SoLuong = SoLuongs[i],
                    KichCo = KichCos[i],
                    DonGia = DonGias[i],
                    TongTien = SoLuongs[i] * DonGias[i]
                });
            }

            // Truyền danh sách sản phẩm vào ViewBag
            ViewBag.DanhSachSanPham = danhSachSanPham;

            return View();
        }

        // POST: Lấy thông tin sản phẩm từ giỏ hàng và xử lý đơn hàng
        [HttpPost]
        public IActionResult PlaceOrder(string userInfoOption, string hoVaTen, string email, string dienThoai, string diaChi, int paymentMethod, List<SanPhamViewModel> DanhSachSanPham)
        {
            // Lấy ID người dùng từ claims
            string userId = User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ThongBaoLoi"] = "Bạn cần đăng nhập để thực hiện hành động này.";
                return RedirectToAction("Login", "Home");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = _context.NguoiDung.FirstOrDefault(u => u.ID.ToString() == userId);

            if (nguoiDung == null)
            {
                TempData["ThongBaoLoi"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login", "Home");
            }

            // Xử lý thông tin người dùng (nếu chọn nhập thông tin mới)
            if (userInfoOption == "custom")
            {
                // Cập nhật thông tin người dùng với các giá trị từ form nhập
                nguoiDung.HoVaTen = hoVaTen ?? nguoiDung.HoVaTen;
                nguoiDung.Email = email ?? nguoiDung.Email;
                nguoiDung.DienThoai = dienThoai ?? nguoiDung.DienThoai;
                nguoiDung.DiaChi = diaChi ?? nguoiDung.DiaChi;

                // Lưu lại thông tin người dùng đã được cập nhật
                _context.SaveChanges();
            }
            else
            {
                // Nếu không chọn "custom", sử dụng thông tin mặc định của người dùng
                hoVaTen = nguoiDung.HoVaTen;
                email = nguoiDung.Email;
                dienThoai = nguoiDung.DienThoai;
                diaChi = nguoiDung.DiaChi;
            }

            // Tạo bản ghi đặt hàng mới
            var datHang = new DatHang
            {
                NguoiDungID = nguoiDung.ID,
                TenKhachHang = hoVaTen,
                DiaChiEmail = email,
                TinhTrangID = 2, // Mặc định là trạng thái "Chờ xử lý"
                DienThoaiGiaoHang = dienThoai,
                DiaChiGiaoHang = diaChi,
                NgayDatHang = DateTime.Now,
                PhuongThucThanhToan = paymentMethod // 1: COD, 2: Thẻ, 3: Trực tuyến
            };

            // Lưu đơn hàng vào cơ sở dữ liệu
            _context.DatHang.Add(datHang);
            _context.SaveChanges();

            // Lưu chi tiết đơn hàng vào bảng DatHangChiTiet
            foreach (var sp in DanhSachSanPham)
            {
                var datHangChiTiet = new DatHangChiTiet
                {
                    DatHangID = datHang.ID, // Lấy ID của đơn hàng đã lưu
                    SanPhamID = sp.ID, // ID sản phẩm từ danh sách sản phẩm
                    SoLuong = (short)sp.SoLuong, // Số lượng sản phẩm
                    DonGia = (int)sp.DonGia, // Đơn giá sản phẩm
                    KichCo = sp.KichCo
                };

                _context.DatHang_ChiTiet.Add(datHangChiTiet);
            }

            // Lưu các chi tiết đơn hàng vào cơ sở dữ liệu
            _context.SaveChanges();

            // Xóa toàn bộ sản phẩm trong giỏ hàng của người dùng sau khi đặt hàng thành công
            var gioHang = _context.GioHang.Where(g => g.TenDangNhap == User.Identity.Name).ToList();
            if (gioHang.Any())
            {
                _context.GioHang.RemoveRange(gioHang);
                _context.SaveChanges();
            }

            // Hiển thị thông báo thành công và chuyển hướng về view dathangthanhcong ở controller Dathang
            TempData["ThongBaoThanhCong"] = "Đặt hàng thành công!";
            return RedirectToAction("DathangThanhCong", "Dathang");
        }


        public ActionResult DathangThanhCong()
        {
            return View(); // Đây sẽ trả về view dathangthanhcong.cshtml
        }
    }

    // ViewModel để hiển thị dữ liệu
    public class SanPhamViewModel
    {
        public int ID { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public string KichCo { get; set; }
        public decimal DonGia { get; set; }
        public decimal TongTien { get; set; }
    }
}

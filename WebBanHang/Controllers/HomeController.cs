using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebBanHang.Models;
using BC = BCrypt.Net.BCrypt;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {

        private readonly FashionShopDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(FashionShopDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        // GET: Login
        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Nếu đã đăng nhập thì chuyển đến trang chủ
                return LocalRedirect(ReturnUrl ?? "/");
            }
            else
            {
                // Nếu chưa đăng nhập thì chuyển đến trang đăng nhập
                ViewBag.LienKetChuyenTrang = ReturnUrl ?? "/";
                return View();
            }
        }

        // POST: Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind] DangNhap dangNhap, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var nguoiDung = _context.NguoiDung.Where(r => r.TenDangNhap == dangNhap.TenDangNhap).SingleOrDefault();

                if (nguoiDung == null || !BC.Verify(dangNhap.MatKhau, nguoiDung.MatKhau))
                {
                    TempData["ThongBaoLoi"] = "Tài khoản không tồn tại trong hệ thống.";
                    return View(dangNhap);
                }
                else
                {
                    // Tạo danh sách claims
                    var claims = new List<Claim>
            {
                new Claim("ID", nguoiDung.ID.ToString()),
                new Claim(ClaimTypes.Name, nguoiDung.TenDangNhap),
                new Claim("HoVaTen", nguoiDung.HoVaTen),
                new Claim(ClaimTypes.Role, nguoiDung.Quyen ? "Admin" : "User")
            };

                    // Tạo identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = dangNhap.DuyTriDangNhap
                    };

                    // Đăng nhập hệ thống
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Lưu thông tin vào session
                    HttpContext.Session.SetString("Username", nguoiDung.TenDangNhap);
                    HttpContext.Session.SetString("UserID", nguoiDung.ID.ToString());  // Lưu ID người dùng vào session
                    HttpContext.Session.SetString("HoVaTen", nguoiDung.HoVaTen); // Lưu Họ và Tên vào session
                    HttpContext.Session.SetString("Email", nguoiDung.Email); // Lưu Email vào session
                    HttpContext.Session.SetString("DienThoai", nguoiDung.DienThoai); // Lưu Điện thoại vào session
                    HttpContext.Session.SetString("DiaChi", nguoiDung.DiaChi); // Lưu Địa chỉ vào session

                    // Điều hướng dựa trên quyền của người dùng
                    if (nguoiDung.Quyen) // Nếu quyền là Admin
                    {
                        return LocalRedirect("/Admin");
                    }
                    else // Nếu quyền là User
                    {
                        return LocalRedirect("/Home");
                    }
                }
            }

            return View(dangNhap);
        }



        // GET: DangXuat
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Xóa thông tin session
            HttpContext.Session.Remove("Username");

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        // GET: Forbidden
        public IActionResult Forbidden()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}




using DoAn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly DataContext _context;

        public DangNhapController(DataContext context)
        {
            _context = context;
        }

        // Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Index()
        {
            return View(new DangNhap());
        }

        // Xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DangNhap model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Tìm tài khoản
            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(t =>
                    t.TK_TenDangNhap == model.TenDangNhap);

            // Kiểm tra mật khẩu
            if (taiKhoan == null ||
                taiKhoan.TK_MatKhau != model.MatKhau)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Tên đăng nhập hoặc mật khẩu không đúng.");

                return View(model);
            }

            // Kiểm tra trạng thái
            if (!taiKhoan.TK_TrangThai)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Tài khoản đã bị khóa.");

                return View(model);
            }

            // Kiểm tra vai trò
            if (taiKhoan.TK_VaiTro != "Admin" &&
                taiKhoan.TK_VaiTro != "NhanVien")
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Vai trò tài khoản không hợp lệ.");

                return View(model);
            }

            // Xóa Session cũ
            HttpContext.Session.Clear();

            // Lưu thông tin tài khoản vào Session
            HttpContext.Session.SetInt32(
                "TaiKhoanID", taiKhoan.TK_ID);

            HttpContext.Session.SetString(
                "TenDangNhap", taiKhoan.TK_TenDangNhap ?? "");

            HttpContext.Session.SetString(
                "HoTen", taiKhoan.TK_HoTen ?? "");

            HttpContext.Session.SetString(
                "VaiTro", taiKhoan.TK_VaiTro ?? "");

            // Kiểm tra Session trong Terminal
            Console.WriteLine(
                "Tên đăng nhập: " +
                HttpContext.Session.GetString("TenDangNhap"));

            Console.WriteLine(
                "Họ tên: " +
                HttpContext.Session.GetString("HoTen"));

            // Chuyển trang theo vai trò
            if (taiKhoan.TK_VaiTro == "Admin")
            {
                return RedirectToAction(
                    "Index", "Home", new { area = "Admin" });
            }

            return RedirectToAction(
                "Index", "Home", new { area = "" });
        }

        // Đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return Redirect("/DangNhap/Index");
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyTaiSan.Models
{
    [Table("tblTaiKhoan")]
    public class tblTaiKhoan
    {
        [Key]
        public int TK_ID { get; set; }

        [Required, StringLength(50)]
        public string TK_TenDangNhap { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string TK_MatKhau { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string TK_HoTen { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string? TK_Email { get; set; }

        [StringLength(15)]
        public string? TK_SoDienThoai { get; set; }

        [Required, StringLength(10)]
        public string TK_VaiTro { get; set; } = "NhanVien";

        public bool TK_TrangThai { get; set; } = true;

        public DateTime TK_NgayTao { get; set; } = DateTime.Now;
    }
}
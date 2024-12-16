using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanHang.Models;

namespace WebBanHang.Models
{
    public class GioHang
    {
        [Key]
        public int ID { get; set; }
        public int SanPhamID { get; set; }

        [StringLength(255)]
        public string TenDangNhap { get; set; }
        public int SoLuongTrongGio { get; set; }

        [StringLength(10)]
        public string KichCo { get; set; }  

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime ThoiGian { get; set; }

        public SanPham? SanPham { get; set; }

        // Thuộc tính tính toán (không lưu vào database)
        [NotMapped]
        public decimal DonGia => SanPham?.DonGia ?? 0;

        [NotMapped]
        public decimal TongTien => DonGia * SoLuongTrongGio;

    }
}

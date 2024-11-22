using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class HangSanXuat
    {
        [DisplayName("Mã Hãng Sản Xuất")]

        public int ID { get; set; }

        [Required(ErrorMessage = "Tên hãng sản xuất không được bỏ trống.")]
        [DisplayName("Tên hãng sản xuất")]

        [StringLength(255)]
        public string TenHangSanXuat { get; set; }
        public ICollection<SanPham>? SanPham { get; set; }
    }
}

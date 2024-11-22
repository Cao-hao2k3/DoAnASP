using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class LoaiSanPham
    {
        [DisplayName("Mã loại sản phẩm")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên loại không được bỏ trống.")]
        [DisplayName("Tên loại sản phẩm")]

        [StringLength(255)]
        public string TenLoai { get; set; }
        public ICollection<SanPham>? SanPham { get; set; }
    }
}

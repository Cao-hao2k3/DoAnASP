using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebBanHang.Models
{
    public class SanPham
    {
        [DisplayName("Mã Sản Phẩm")]
        public int ID { get; set; }
        [DisplayName("Hãng sản xuất")]
        [Required(ErrorMessage = "Hãng sản xuất không được bỏ trống.")]
        public int HangSanXuatID { get; set; }
        [DisplayName("Loại sản phẩm")]
        [Required(ErrorMessage = "Loại sản phẩm không được bỏ trống.")]
        public int LoaiSanPhamID { get; set; }

        [StringLength(255)]
        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống.")]
        public string TenSanPham { get; set; }

        [DisplayName("Đơn giá")]
        [Required(ErrorMessage = "Đơn giá không được bỏ trống.")]
        /*
         * Dùng để định dạng số thành chuỗi, sử dụng kiểu "N0".
            N0: Hiển thị số với phân cách hàng nghìn, không có phần thập phân.
            Ví dụ: 1000000 → "1,000,000" (hoặc định dạng tương ứng với ngôn ngữ hệ thống, ví dụ: "1.000.000").
            ApplyFormatInEditMode = false:
            Quy định việc định dạng chỉ áp dụng khi hiển thị giá trị trong chế độ xem (View) hoặc hiển thị dữ liệu.
            Trong chế độ chỉnh sửa (Edit Mode), giá trị sẽ không được định dạng.
            Ví dụ: Khi chỉnh sửa giá trị trong form, nó có thể hiện thị là 1000000 thay vì 1,000,000.
         */
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        public int DonGia { get; set; }

        [DisplayName("Số lượng")]
        [Required(ErrorMessage = "Số lượng không được bỏ trống.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        public int SoLuong { get; set; }

        [StringLength(10)]
        [DisplayName("Size")]
        [Required(ErrorMessage = "Size không được bỏ trống.")]
        public string KichCo { get; set; }


        [StringLength(255)]
        [DisplayName("Hình ảnh")]
        public string? HinhAnh { get; set; }

        [NotMapped]
        [Display(Name = "Hình ảnh sản phẩm")]
        public IFormFile? DuLieuHinhAnh { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Mô tả chi tiết")]
        [DataType(DataType.MultilineText)]
        public string? MoTa { get; set; }

        public LoaiSanPham? LoaiSanPham { get; set; }
        public HangSanXuat? HangSanXuat { get; set; }
        public ICollection<DatHangChiTiet>? DatHang_ChiTiet { get; set; }

        public ICollection<GioHang>? GioHang { get; set; }
    }
}

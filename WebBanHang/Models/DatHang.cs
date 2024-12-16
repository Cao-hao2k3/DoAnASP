            using System.ComponentModel;
            using System.ComponentModel.DataAnnotations;

            namespace WebBanHang.Models
            {
                public class DatHang
                {
                    [DisplayName("Mã Đặt Hàng")]

                    public int ID { get; set; }

                    [DisplayName("Mã khách hàng")]
                    [Required(ErrorMessage = "Khách hàng không được bỏ trống.")]
                    public int NguoiDungID { get; set; }

                    [DisplayName("Tên Khách Hàng")]
                    [Required(ErrorMessage = "Khách hàng không được bỏ trống.")]
                    public string TenKhachHang { get; set; }

                    [DisplayName("Email")]
                    [Required(ErrorMessage = "Khách hàng không được bỏ trống.")]
                    public string? DiaChiEmail { get; set; }

                    [DisplayName("Tình trạng")]
                    [Required(ErrorMessage = "Tình trạng không được bỏ trống.")]
                    public int TinhTrangID { get; set; }

                    [DisplayName("Điện thoại giao hàng")]
                    [Required(ErrorMessage = "Điện thoại giao hàng không được bỏ trống.")]
                    [StringLength(10, ErrorMessage = "Số điện thoại phải là {1} ký tự.", MinimumLength = 10)]
                    [RegularExpression(@"^\d+$", ErrorMessage = "Điện thoại giao hàng chỉ được chứa các chữ số.")]
                    public string DienThoaiGiaoHang { get; set; }


                    [StringLength(255)]
                    [DisplayName("Địa chỉ giao hàng")]
                    [Required(ErrorMessage = "Địa chỉ giao hàng không được bỏ trống.")]
                    public string DiaChiGiaoHang { get; set; }

                    [DisplayName("Ngày đặt hàng")]
                    [Required(ErrorMessage = "Ngày đặt hàng không được bỏ trống.")]
                    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
                    
                    public DateTime NgayDatHang { get; set; }

                     [DisplayName("Phương thức thanh toán")]
                    public int PhuongThucThanhToan { get; set; } // 1: COD, 2: Thẻ, 3: Trực tuyến

                    public NguoiDung? NguoiDung { get; set; }
                    public TinhTrang? TinhTrang { get; set; }
                    public ICollection<DatHangChiTiet>? DatHang_ChiTiet { get; set; }
                }
            }

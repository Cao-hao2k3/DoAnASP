using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class TinhTrang
    {
        [DisplayName("Mã tình trạng")]
        public int ID { get; set; }

        [StringLength(255)]
        [DisplayName("Tình trạng")]
        [Required(ErrorMessage ="Tên tình trạng không được bỏ trống !")]
        public string MoTa { get; set; }
        public ICollection<DatHang>? DatHang { get; set; }
    }
}

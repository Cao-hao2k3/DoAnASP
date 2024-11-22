using Microsoft.EntityFrameworkCore;

namespace WebBanHang.Models
{
    public class FashionShopDbContext : DbContext   
    {
        public FashionShopDbContext(DbContextOptions<FashionShopDbContext> options) : base(options) { }

        public DbSet<LoaiSanPham> LoaiSanPham { get; set; }
        public DbSet<HangSanXuat> HangSanXuat { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<TinhTrang> TinhTrang { get; set; }
        public DbSet<DatHang> DatHang { get; set; }
        public DbSet<DatHangChiTiet> DatHang_ChiTiet { get; set; }
    }
}

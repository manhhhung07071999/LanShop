using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ChiTiet : MatHang
    {
        new public string MaHang { get; set; }
        public bool MuaLe { get; set; }
        public int SoLuong { get; set; }
        public double Tong { get { return TongDonNguyen * DonGia; } }
        public double TongSauVAT { get { return Tong * (100 + VAT) / 100; } }
        public string QuyCach
        {
            get
            {
                if (base.DongGoi == null || MuaLe)
                {
                    return base.LoaiDonNguyen;
                }
                return string.Format("{0} ({1} {2})", base.DongGoi, SoDonNguyen, base.LoaiDonNguyen);
            }
        }
        public int TongDonNguyen
        {
            get
            {
                if (MuaLe) return SoLuong;
                return SoDonNguyen * SoLuong;
            }
        }
    }
    public class HoaDon : BsonData.Document
    {
        public string Ma { set; get; }
        public DateTime Ngay { get; set; }
        public double Tong { get; set; }
        public double VAT { get; set; }
        public double TongSauVAT { get { return Tong * (100 + VAT) / 100; } }

        public List<ChiTiet> ChiTiet { get; set; } = new List<ChiTiet>();
        public string KhachHang { get; set; }
    }
}

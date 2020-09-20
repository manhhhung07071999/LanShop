using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Models
{
    public class ChiTiet : MatHang
    {
        public string GetKey()
        {
            var key = MaHang;
            if (MuaLe) key += "_1";

            return key;
        }
        public bool MuaLe { get; set; }
        public string MaHang { get; set; }
        public int SoLuong { get; set; }
        public double Tong { get { return TongDonNguyen * DonGia; } }
        public double TongSauVAT { get { return Tong * (100 + VAT) / 100;  } }
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
        public string KhachHang { get; set; }
        public double Tong { get; set; }
        public double VAT { get; set; }
        public double TongSauVAT { get { return Tong * (100 + VAT) / 100; } }

        public List<ChiTiet> ChiTiet { get; set; } = new List<ChiTiet>();
    }

    public class HoaDonDocument
    {
        public string Html { get; set; }
    }

    public class MapChiTiet : Dictionary<string, ChiTiet>
    {
        public void Update(Vst.UpdateActions action, ChiTiet item)
        {
            var key = item.GetKey();
            switch (action)
            {
                case Vst.UpdateActions.Insert:
                    if (this.ContainsKey(key))
                        this[key].SoLuong += item.SoLuong;
                    else
                        this.Add(key, item);
                    break;

                case Vst.UpdateActions.Delete:
                    if (this.ContainsKey(key))
                        this.Remove(key);
                    break;
            }
            RaiseChanged();
        }
        
        public event Action<MapChiTiet> Changed;
        public void RaiseChanged()
        {
            Changed?.Invoke(this);
        }
    }
}

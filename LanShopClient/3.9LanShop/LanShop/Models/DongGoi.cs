using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Models
{
    public class DongGoi : BsonData.Document
    {
        public override string ToString()
        {
            if (SoLuong < 2) return Loai;

            return string.Format("{0}/{1}", SoLuong, Loai);
        }
        public int SoLuong { get; set; }

        public string Loai { get; set; }
    }
}

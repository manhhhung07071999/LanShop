using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Models
{
   public class MatHang: BsonData.Document
    {
        //public string Ma { get; set; }
        public string MaVach { get; set; }
        public string Ten{ get; set; }
        public int DonGia { get; set; }
        public string MoTa { get; set; }
        public string DongGoi { get; set; }
        public int SoDonNguyen { get; set; }
        public string LoaiDonNguyen { get; set; }
        public int VAT { get; set; }
    }
}

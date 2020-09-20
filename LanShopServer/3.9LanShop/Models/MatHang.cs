using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class MatHang: BsonData.Document
    {
        public string MaHang
        {
            get { return Id; }
            set { Id = value; }
        }
        public string MaVach { get; set; }
        public string Ten{ get; set; }
        public int DonGia { get; set; }
        public string MoTa { get; set; }
        public string DongGoi { get; set; } = "thùng";
        public int SoDonNguyen { get; set; } = 1;
        public string LoaiDonNguyen { get; set; } = "hộp";
        public int VAT { get; set; }
    }
}

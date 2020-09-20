using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Models
{
    public class KhachHang:BsonData.Document
    {
        public string Ten { get; set; }
        public string SoDT { get; set; }
        

    }
}

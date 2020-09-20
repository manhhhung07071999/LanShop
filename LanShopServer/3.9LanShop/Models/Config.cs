using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Config
    {
        public string ServerIp { get; set; }
        public string Company { get; set; }
        public bool InvoiceHasCompany { get; set; } = true;
    }
}

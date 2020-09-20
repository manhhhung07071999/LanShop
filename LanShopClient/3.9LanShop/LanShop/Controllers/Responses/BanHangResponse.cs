using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Controllers.Responses
{
    class BanHangResponseController : Controller
    {
        public object TimMatHang()
        {
            return Vst.Json.GetObject<Models.ChiTiet>(MessageContext.Message);
        }
    }
}

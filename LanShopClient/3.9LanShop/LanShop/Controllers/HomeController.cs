using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Controllers
{
    class HomeController : Controller
    {
        public System.Mvc.ActionResult Default()
        {
            
            return Redirect("setup/detect");
        }
    }
}

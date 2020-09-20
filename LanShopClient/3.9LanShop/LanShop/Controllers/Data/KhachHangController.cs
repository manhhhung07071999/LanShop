using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Vst;

namespace LanShop.Controllers.Data
{
    class KhachHangController : Data.DataController<Models.KhachHang>
    {
        public ActionResult Default()
        {
            return View(GetAll());
        }
        public override ActionResult Update(UpdateRequest request)
        {
            if (request.ObjectId == null)
            {
                var kh = (Models.KhachHang)request.Value;
                request.ObjectId = kh.SoDT;

               
            }
            return base.Update(request);
        }
    }
}

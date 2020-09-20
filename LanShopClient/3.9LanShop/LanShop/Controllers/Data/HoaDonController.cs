using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Vst;

namespace LanShop.Controllers.Data
{
     class HoaDonController:Data.DataController<Models.HoaDon>
    {
        public ActionResult Default()
        {
            return View(GetAll());
        }
        public override ActionResult Update(UpdateRequest request)
        {
            if (request.ObjectId == null)
            {
                var hd = (Models.HoaDon)request.Value;
                request.ObjectId = hd.Ma;


            }
            return base.Update(request);
        }
    }
}

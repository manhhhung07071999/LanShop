using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using Vst;

namespace LanShop.Controllers.Data
{
    class MatHangController : Data.DataController< Models.MatHang>
    {
        public ActionResult Default()
        {
            return View(GetAll());
        }
        public override ActionResult Update(UpdateRequest request)
        {
            if( request.ObjectId == null)
            {
                var mh = (Models.MatHang)request.Value;
                request.ObjectId = mh.Id;

                if(Collection.Contains(mh.Id))
                {
                    return Message("mahang");
                }

            }
            return base.Update(request);
        }
    }
}

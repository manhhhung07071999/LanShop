using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;

using Vst;

namespace LanShop.Controllers.Data
{
    using Models;
    class MatHangController : Data.DataController<Models.MatHang>
    {
        public ActionResult Default()
        {
            return View(GetAll());
        }

        public override ActionResult Update(UpdateRequest request)
        {
            var mh = (MatHang)request.Value;
            if ( request.ObjectId == null)
            {
                request.ObjectId = mh.Id;

                if(Collection.Contains(mh.Id))
                {
                    return Message("mahang");
                }

            }

            if (request.Action != UpdateActions.Delete)
            {
                var db = Models.Data.RemovableDB.GetCollection<DongGoi>();
                db.FindAndUpdate<DongGoi>("goi", e => {
                    return e.Add(mh.DongGoi);
                });
                db.FindAndUpdate<DongGoi>("loai", e => {
                    return e.Add(mh.LoaiDonNguyen);
                });
            }
            return base.Update(request);
        }

        public ActionResult GetDongGoi()
        {
            var db = Models.Data.RemovableDB.GetCollection<DongGoi>();
            try
            {
                ViewData["dong-goi"] = db.FindById<DongGoi>("goi").Names;
                ViewData["loai-don-nguyen"] = db.FindById<DongGoi>("loai").Names;
            }
            catch
            {
                db.Insert("goi", new DongGoi());
                db.Insert("loai", new DongGoi());
            }

            return Done();
        }
    }
}

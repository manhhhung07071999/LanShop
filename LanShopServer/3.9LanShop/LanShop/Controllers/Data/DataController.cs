using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vst;
using MVC = System.Mvc;
using Models.Data;

namespace LanShop.Controllers.Data
{
    class DataController : Controller
    {
        public DataController()
        {
        }
    }

    class DataController<T> : DataController
        where T: new()
    {
        BsonData.Collection _collection;
        protected BsonData.Collection Collection => _collection;

        public DataController()
        {
            _collection = RemovableDB.GetCollection<T>();
        }

        /// <summary>
        /// Hàm lấy tất cả dữ liệu
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<T> GetAll()
        {
            return Collection.ToList<T>();
        }

        /// <summary>
        /// Hàm tìm bản ghi theo Id
        /// Có thể viết lại (override) để thực hiện các mối quan hệ (relationships)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual T Find(string id)
        {
            return Collection.FindById<T>(id);
        }

        #region Cập nhật
        /// Có thể viết lại (override) để thực hiện các mối quan hệ (relationships)
        public virtual MVC.ActionResult Create()
        {
            return View(new UpdateRequest
            {
                Action = UpdateActions.Insert,
                Value = new T(),
            });
        }
        public virtual MVC.ActionResult Edit(string id)
        {
            return View(new UpdateRequest
            {
                Action = UpdateActions.Update,
                ObjectId = id,
                Value = Find(id)
            });
        }

        public MVC.ActionResult Delete(string id)
        {
            return View(new UpdateRequest
            {
                Action = UpdateActions.Delete,
                ObjectId = id,
                Value = Find(id)
            });
        }

        public virtual MVC.ActionResult Update(UpdateRequest request)
        {
            Collection.Update(request);
            return GoFirst();
        }
        #endregion
    }
}

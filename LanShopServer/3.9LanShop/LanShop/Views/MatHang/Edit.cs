using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//use
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.MatHang
{
    class Edit : Editor<Models.MatHang>
    {
        static BindingInfoCollection _bindingInfos;
        protected override BindingInfoCollection GetBindingInfo(string name)
        {
            if (_bindingInfos == null)
            {
                _bindingInfos = base.GetBindingInfo(name).Clone();
            }

            return _bindingInfos;
        }

        protected override void RenderInputs(BindingInfoCollection infos)
        {
            Controller.Execute("GetDongGoi");
            MainContent.LayoutInputs = Layout;

            base.RenderInputs(infos);
        }
        void Layout(ControlBox box)
        {
            box.SetWidths(120, 10, 100, 10, 200, 10, 250);
            box.SetHeights(GridUnitType.Star, 1, 1, 1, 1, 1, 1, 1, 1);

            var cbGoi = (MyComboBox)box["DongGoi"];
            cbGoi.ItemsSource = ViewBag["dong-goi"];

            var cbLoai = (MyComboBox)box["LoaiDonNguyen"];
            cbLoai.ItemsSource = ViewBag["loai-don-nguyen"];

            this.SetInputPosition(box["MaHang"], 0, 0);
            this.SetInputPosition(box["MaVach"], 0, 2, 3, 0);
            this.SetInputPosition(box["Ten"], 2, 0, 5, 0);
            this.SetInputPosition(box["DonGia"], 4, 0, 3, 0);
            this.SetInputPosition(box["VAT"], 4, 4);
            this.SetInputPosition(cbGoi, 6, 0);
            this.SetInputPosition(box["SoDonNguyen"], 6, 2);
            this.SetInputPosition(cbLoai, 6, 4);
            this.SetInputPosition(box["MoTa"], 0, 6, 0, 8);


        }
    }

    class Create : Edit { }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//use
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.BanHang
{
    class ShowMatHang : Editor<Models.ChiTiet>
    {
        static BindingInfoCollection _bindingInfos;
        protected override BindingInfoCollection GetBindingInfo(string name)
        {
            if (_bindingInfos == null)
            {
                _bindingInfos = base.GetBindingInfo("MatHang").Clone();
            }

            return _bindingInfos;
        }

        protected override void RenderInputs(BindingInfoCollection infos)
        {
            //MainContent.LayoutInputs = Layout;
            MainContent.Orientation = Orientations.Horizontal;
            base.RenderInputs(infos);
        }

        protected override Dialog CreateDialog(string accept, string cancel)
        {
            var dlg = base.CreateDialog(accept, cancel);
            if (Model.Action == Vst.UpdateActions.Delete)
            {
                dlg.AcceptButton.Css = "danger";
                dlg.AcceptButton.Text = "Xóa";
            }
            return dlg;
        }
        void Layout(ControlBox box)
        {
            
            //box.SetWidths(120, 10, 100, 10, 200, 10, 250);
            //box.SetHeights(GridUnitType.Star, 1, 1, 1, 1, 1, 1, 1, 1);

            //this.SetInputPosition(box["MaHang"], 0, 0);
            //this.SetInputPosition(box["MaVach"], 0, 2, 3, 0);
            //this.SetInputPosition(box["Ten"], 2, 0, 5, 0);
            //this.SetInputPosition(box["DonGia"], 4, 0, 3, 0);
            //this.SetInputPosition(box["VAT"], 4, 4);
            //this.SetInputPosition(box["DongGoi"], 6, 0);
            //this.SetInputPosition(box["SoDonNguyen"], 6, 2);
            //this.SetInputPosition(box["LoaiDonNguyen"], 6, 4);
            //this.SetInputPosition(box["MoTa"], 0, 6, 0, 8);
        }
    }
}

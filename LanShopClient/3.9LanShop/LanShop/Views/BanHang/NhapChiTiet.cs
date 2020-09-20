using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.BanHang
{
    class NhapChiTiet : Editor<object>, System.Mvc.IAsyncView
    {
        static object _maHang;
        
        protected override BindingInfoCollection GetBindingInfo(string name)
        {
            return base.GetBindingInfo("TimHang");
        }
        IMyInput Input => MainContent.Inputs[0];

        protected override void RenderInputs(BindingInfoCollection infos)
        {
            base.RenderInputs(infos);
            Input.Value = _maHang;
        }

        protected override Dialog CreateDialog(string accept, string cancel)
        {
            var dlg = base.CreateDialog(accept, cancel);

            dlg.Activated += (s, e) => ((MyTextBox)Input).Input.SelectAll();

            return dlg;
        }

        protected override bool UpdateModel(string actionName)
        {
            Controller.Execute("TimMatHang", _maHang = Input.Value);
            return false;
        }
    }
}

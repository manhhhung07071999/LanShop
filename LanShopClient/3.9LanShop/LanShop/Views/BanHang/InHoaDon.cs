using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.BanHang
{
    class InHoaDon : Renderer<Models.HoaDonDocument>
    {
        public StyleElement _caption()
        {
            var cap = new MyLabel {
                Text = "In hóa đơn",
                Css = "content-caption"
            };
            //btn.Click += e => {
            //    _wb.InvokeScript("execScript", new object[] { "window.print();", "JavaScript" });
            //    MyApp.Execute("banHang");
            //};
            var grid = new PanelElement<Grid> {
            };
            grid.Add(cap);

            return grid;
        }

        WebBrowser _wb;
        public WebBrowser _browser()
        {
            _wb = new WebBrowser();
            _wb.NavigateToString(Model.Html);

            return _wb;
        }
    }
}

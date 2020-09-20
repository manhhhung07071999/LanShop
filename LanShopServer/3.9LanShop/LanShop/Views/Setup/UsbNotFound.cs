using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LanShop.Views.Setup
{
    class UsbNotFound : Renderer<object>
    {
        public override object GetResult()
        {
            var dlg = new Dialog(GetTextByCode("usb-0"), "Close", null);
            dlg.ShowDialog();

            MyApp.Browser.Close();
            return null;
        }
    }
}

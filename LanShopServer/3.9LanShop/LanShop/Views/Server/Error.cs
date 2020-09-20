using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanShop.Views.Server
{
    class Error : Message
    {
        protected override string GetBodyText()
        {
            return "Không thể chạy server tại địa chỉ " + Model;
        }
        protected override void OnClosing()
        {
            MyApp.Execute("Setup/Edit");
        }
    }
}

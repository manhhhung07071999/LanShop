using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.HoaDon
{
    class Default : DataRenderer<Models.HoaDon>
    {
        protected override UIElement CreateCaption(string caption, UIElement[] buttons)
        {
            return base.CreateCaption("Danh Sách Hóa Đơn", buttons);
        }
    }
}

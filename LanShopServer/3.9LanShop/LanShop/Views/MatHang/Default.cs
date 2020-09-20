using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.MatHang
{
    class Default : DataRenderer<Models.MatHang>
    {
        protected override UIElement CreateCaption(string caption, UIElement[] buttons)
        {
            return base.CreateCaption("Danh sách mặt hàng", buttons);
        }
    }
}

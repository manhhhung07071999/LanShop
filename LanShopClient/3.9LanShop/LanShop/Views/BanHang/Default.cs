using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.BanHang
{
    using Models;
    class Default : Renderer<MapChiTiet>
    {
        public override void ProcessResponse(object value)
        {
            if (value is ChiTiet)
            {
                Controller.Execute("showMatHang", Vst.UpdateActions.Insert, (ChiTiet)value);
            }
        }
        MyLabel _caption()
        {
            return new MyLabel
            {
                Css = "content-caption",
                Text = "Hóa đơn"
            };
        }

        StackPanel _chucNang()
        {
            var btnThem = new MyButton
            {
                Text = "Nhập thêm",
                Url = "banHang/nhapChiTiet"
            };

            var btnThanhToan = new MyButton
            {
                Text = "Thanh toán",
                Url = "banHang/showHoaDon",
                Css = "warning",
            };
            btnThanhToan.Click += e =>
            {

            };

            var sp = new StackPanel {
                Orientation = Orientation.Horizontal,
                Children = { btnThem, btnThanhToan },
                Margin = new Thickness(10),
            };

            var padding = new Bound(15, 10);
            foreach (MyButton btn in sp.Children)
            {
                btn.Style.Padding = padding;
            }
            return sp;
        }

        MyGridView gridChiTiet;
        MyGridView _chiTiet()
        {
            var grid = new MyGridView
            {
                Binding = "CTHD",
                ItemsSource = Model,
            };

            Model.Changed += data => grid.ItemsSource = data.Values;
            grid.OpenItem += (i) => {
                Controller.Execute("showMatHang", Vst.UpdateActions.Update, i);
            };
            grid.DeleteItem += (i) => {
                 Controller.Execute("showMatHang", Vst.UpdateActions.Delete, i);             
            };
            return gridChiTiet = grid;
        }

        protected override void LoadElements()
        {
            base.LoadElements();
            MainContent.AddRow();

            Controller.Execute("NhapChiTiet");
        }
    }
}

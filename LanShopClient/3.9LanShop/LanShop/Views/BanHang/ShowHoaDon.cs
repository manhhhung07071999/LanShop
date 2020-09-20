using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.BanHang
{
    class ShowHoaDon : Renderer<Models.HoaDon>
    {
        MyTextBox _khachHang;
        protected override void LoadElements()
        {
            MainContent.SetWidths(150, 350);
            BindingInfoCollection infos = "HoaDon";

            var type = typeof(Models.HoaDon);
            foreach (var p in infos)
            {
                var info = p.Value;
                var r = MainContent.AddRow();
                MainContent.RowDefinitions[r].Height = new GridLength(40);

                MainContent.Add(r, 0, new MyLabel {
                    Text = info.Caption,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = (Thickness)(new Bound(15, 5)),
                });

                StyleElement elem = null;
                object v = type.GetProperty(p.Key).GetValue(Model);
                
                if (info.Input == null)
                {
                    elem = new MyLabel
                    {
                        Text = info.ToString(v),
                        HorizontalAlignment = HorizontalAlignment.Left,
                    };
                    elem.Css = info.Type ?? "hoadon-item";
                }
                else
                {
                    elem = _khachHang = new MyTextBox();
                }
                MainContent.Add(r, 1, elem);
            }

            MainContent.AddRow();
            MainContent.Margin = new Thickness(0, 20, 10, 20);
        }

        public override object GetResult()
        {
            var dlg = new Dialog(null, "OK", "Cancel") {
                Body = MainContent,
            };
            dlg.AcceptButton.Click += (e) => {
                Model.KhachHang = (string)_khachHang.Value;
                Controller.Execute("chotHoaDon", Model);
            };
            dlg.ShowDialog();

            return dlg;
        }
    }
}

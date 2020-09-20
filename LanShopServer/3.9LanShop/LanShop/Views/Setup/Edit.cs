using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Models;

namespace LanShop.Views.Setup
{
    class Edit : Editor<Config>
    {
        protected override void RenderInputs(BindingInfoCollection infos)
        {
            MainContent.Orientation = Orientations.Horizontal;
            base.RenderInputs(infos);

            MainContent.SetWidths(180, 350);
        }

    }
    class Default : Edit
    {
        protected override Dialog CreateDialog(string accept, string cancel)
        {
            return base.CreateDialog("OK", null);
        }
    }
    class Restart : Renderer<object>
    {
        public override void Render(Controller controller)
        {
            var dlg = new Dialog(GetTextByCode("serverip"));
            dlg.AcceptButton.Click += (e) => {
                var path = MyApp.MapPath("lanshop");
                Application.Current.Exit += (s, v) => {
                    System.Diagnostics.Process.Start(path);
                };
                Application.Current.Shutdown();
            };
            dlg.ShowDialog();
        }
    }
}

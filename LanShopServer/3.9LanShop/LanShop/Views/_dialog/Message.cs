using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LanShop.Views
{
    class Message : Renderer<object>
    {
        public override object GetResult()
        {
            var dlg = new Dialog(GetBodyText() ?? GetTextByCode((string)Model));

            dlg.AcceptButton.Click += e => {
                OnClosing();
            };
            return dlg;
        }
        protected virtual void OnClosing() { }
        protected virtual string GetBodyText()
        {
            return null;
        }

        protected override void LoadElements()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Controls
{
    class MyImage : StyleElement<Image>
    {
        public string Key { get; set; }
        public override void SetContentValue(DependencyProperty property, object value)
        {
            if (property == Control.ForegroundProperty)
            {
                var color = (Media.Color)((Rgb)value);
                this.Content.Source = new ImageRenderer(Key).ToImage(color);

                return;
            }
            base.SetContentValue(property, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace System.Windows.Controls
{

    public class MyButtonEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }

    public class MyButtonBase : MyLabel
    {
        int _state;
        public MyButtonBase()
        {
            this.PreviewMouseLeftButtonDown += (s, e) => {
                _state = 1;
            };
            this.PreviewMouseLeftButtonUp += (s, e) => {
                if (_state != 0)
                {
                    var ev = new MyButtonEventArgs();
                    Click?.Invoke(ev);

                    if (ev.Handled == false && Url != null)
                    {
                        System.Mvc.Engine.Execute(this.Url);
                    }
                }
                _state = 0;
            };
        }

        public event Action<MyButtonEventArgs> Click;
        public string Url { get; set; }
    }

    public class MyButton : MyButtonBase
    {
        public MyButton()
        {
            Css = "primary";

            BorderThickness = new Thickness(1);
            CornerRadius = new CornerRadius(4);

            MinHeight = 30;
            MinWidth = 65;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Controls
{
    public enum MyLayoutOptions {
        TopMenu = 1,
        NavMenu,
        Default,
    };

    public class MyCaptionBox : MyLabel
    {
        public MyCaptionBox()
        {
            Css = "app-caption";
        }
    }
    public class MyLayout : MyTableLayout, IUpdateView
    {
        protected MyCaptionBox _captionBox;
        protected MyTopMenu _topMenu;
        protected MyNavMenu _navMenu;
        protected Border _mainContent;

        public MyCaptionBox CaptionBox => _captionBox;
        public MyTopMenu TopMenu => _topMenu;
        public MyNavMenu NavMenu =>  _navMenu;
        public Border MainContent => _mainContent;

        public void UpdateView(object value)
        {
            var c = _mainContent.Child as IDisposable;

            if (c != null) { c.Dispose(); }

            var e = (UIElement)value;
            e.SetValue(Control.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            e.SetValue(Control.VerticalAlignmentProperty, VerticalAlignment.Stretch);

            _mainContent.Child = e;
        }

        public MyLayout() : this(MyLayoutOptions.Default)
        {
        }
        public MyLayout(MyLayoutOptions options)
        {
            _mainContent = new MyLabel {
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            _captionBox = new MyCaptionBox();


            SetWidths(250, 0);
            SetHeights(60, 0);

            _mainContent.SetValue(RowProperty, 1);
            Children.Add(_mainContent);
            Children.Add(_captionBox);

            if (options.HasFlag(MyLayoutOptions.TopMenu))
            {
                _topMenu = new MyTopMenu();
                _topMenu.SetValue(ColumnProperty, 1);

                Children.Add(_topMenu);
            }
            if (options.HasFlag(MyLayoutOptions.NavMenu))
            {
                _navMenu = new MyNavMenu();
                _navMenu.SetValue(RowProperty, 1);

                _mainContent.SetValue(ColumnProperty, 1);
                Children.Add(_navMenu);
            }
            else
            {
                _mainContent.SetValue(ColumnSpanProperty, 2);
            }
        }

        public string ApplicationText
        {
            get { return _captionBox.Text; }
            set {  _captionBox.Text = value; }
        }
    }
}

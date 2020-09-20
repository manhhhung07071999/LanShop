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

            this.AddRow();
            this.AddColumn();

            Add(_captionBox);

            int r = 0, c = 0;
            if (options.HasFlag(MyLayoutOptions.TopMenu))
            {
                this.AddColumn();
                Add(0, 1, _topMenu = new MyTopMenu());

                r = 1;
            }
            if (options.HasFlag(MyLayoutOptions.NavMenu))
            {
                this.AddRow();
                Add(1, 0, _navMenu = new MyNavMenu());

                c = 1;
            }

            Add(r, c, _mainContent);
            if (c == 0 && r == 1)
            {
                AddRow();
                _mainContent.SetValue(Grid.ColumnSpanProperty, 2);
            }
            if (r == 0 && c == 1)
            {
                AddColumn();
                _mainContent.SetValue(Grid.RowSpanProperty, 2);
            }

            this.SetHeights(60);
            this.SetWidths(250);
        }

        public string ApplicationText
        {
            get { return _captionBox.Text; }
            set {  _captionBox.Text = value; }
        }
    }
}

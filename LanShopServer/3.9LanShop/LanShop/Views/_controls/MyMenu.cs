using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class MyMenuItemInfo
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string ClassName { get; set; }
        public string IconName { get; set; }
        public bool BeginGroup { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool Disabled { get; set; }
        public bool IsActive { get; set; }
        public List<MyMenuItemInfo> Childs { get; set; }
    }

}
namespace System.Windows.Controls
{

    public class MyMenuItem : MyButtonBase
    {
        MyMenuItemInfo _data;
        public MyMenuItemInfo Data => _data;
        public MyMenuItem(MyMenuItemInfo info)
        {
            _data = info;
        }
    }
    public abstract class MyMenu : PanelElement<StackPanel>
    {
        protected abstract MyMenuItem CreateItem(MyMenuItemInfo info);
        public event Action<MyMenuItem> ItemActivated;

        public MyMenuItem Add(MyMenuItemInfo info)
        {
            var item = CreateItem(info);

            item.Text = info.Text;
            item.Url = info.Url;

            return Add(item);
        }
        public MyMenuItem Add(MyMenuItem item)
        {
            base.Add(item);

            item.Click += (e) => {
                ItemActivated?.Invoke(item);
            };

            MenuItemAdded?.Invoke(item);
            return item;
        }
        public MyMenuItem Add(string text, string url)
        {
            var item = CreateItem(new MyMenuItemInfo { Text = text, Url = url });
            item.Text = text;
            item.Url = url;

            return Add(item);
        }

        public event Action<MyMenuItem> MenuItemAdded;

        System.Collections.IEnumerable _itemsSource;
        public System.Collections.IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource == value) { return; }
                this.Content.Children.Clear();

                if ((_itemsSource = value) != null)
                {
                    foreach (var obj in value)
                    {
                        var info = obj as MyMenuItemInfo;
                        if (info == null)
                        {
                            info = Vst.Json.Convert<MyMenuItemInfo>(obj);
                        }
                        this.Add(info);
                    }
                }
            }
        }

        public MyMenuItem this[int index]
        {
            get { return (MyMenuItem)this.Content.Children[index]; }
        }
        public MyMenu()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            this.Css = "menu";
        }
    }

    public class MyTopMenu : MyMenu
    {
        class MI : MyMenuItem
        {
            public MI(MyMenuItemInfo info) : base(info)
            {
                Css = "top-menu-item";
                Content.Margin = new Thickness(10, 0, 10, 0);
            }
        }
        public MyTopMenu()
        {
            this.Content.Orientation = Orientation.Horizontal;
        }
        protected override MyMenuItem CreateItem(MyMenuItemInfo info)
        {
            return new MI(info);
        }
    }

    public class MyNavMenu : MyMenu
    {
        class MI : MyMenuItem
        {
            public MI(MyMenuItemInfo info) : base(info)
            {
                BorderThickness = new Thickness(0, info.BeginGroup ? 1 : 0, 0, 1);
                Content.Margin = new Thickness(20, 10, 0, 10);
                Content.HorizontalAlignment = HorizontalAlignment.Left;

                if (info.BeginGroup)
                {
                    this.Margin = new Thickness(0, 40, 0, 0);
                }

                Css = "nav-menu-item";                
            }
        }
        public MyNavMenu()
        {
            MenuItemAdded += item => {
                var key = item.Data.IconName;
                var m = item.Content.Margin;

                if (key != null)
                {
                    this.Content.Children.Remove(item);
                    var image = new Image {
                        Source = new ImageRenderer(key).ToImage((Media.Color)item.Style.TextColor),
                        Width = 24,
                        Height = 24,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(5, item.Margin.Top + m.Top, 5, m.Bottom),
                    };
                    var grid = new Grid {
                        Children = {
                            item, image
                        }
                    };
                    item.Content.Margin = new Thickness(image.Width + 10, m.Top, 0, m.Bottom);

                    base.Add(grid);
                }
            };
        }
        protected override MyMenuItem CreateItem(MyMenuItemInfo info)
        {
            return new MI(info);
        }
    }
}

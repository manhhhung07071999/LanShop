using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views
{
    class DataRenderer<TModel> : Renderer<IEnumerable<TModel>>
    {
        protected virtual UIElement CreateCaption(string caption, UIElement[] buttons)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(5),
            };
            if (buttons != null)
            {
                foreach (var btn in buttons)
                {
                    stackPanel.Children.Add(btn);
                }
            }
            return new Grid
            {
                Children = {
                    new MyLabel { Text = caption, Css = "content-caption" },
                    stackPanel
                }
            };
        }
        public virtual MyGridView CreateBody(string binding)
        {
            var table = new MyGridView
            {
                Binding = binding,
            };

            table.DoubleClick += (e) => {
                Controller.Execute("edit", ((BsonData.IDocument)e).Id);
            };
            table.ItemRowCreated += (r, e) => {

                var sp = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 0, 10, 0),
                    Visibility = Visibility.Hidden,
                };
                var i = r.Content.AddColumn();
                r.Content.Add(0, i, sp);

                foreach (var b in CreateItemFuncs((TModel)e))
                {
                    b.Margin = new Thickness(0, 0, 4, 0);
                    sp.Children.Add(b);
                }

                r.MouseMove += (s, ev) => { sp.Visibility = Visibility.Visible; };
                r.MouseLeave += (s, ev) => { sp.Visibility = Visibility.Hidden; };
            };

            table.ItemsSource = Model;

            return table;
        }

        protected virtual List<MyButton> CreateItemFuncs(TModel item)
        {
            var del = new MyButton
            {
                Text = "Xóa",
                Css = "danger",
            };
            del.Click += (ev) => {
                if (new Dialog("Xóa bản ghi?", "Có", "Không")
                    .ShowDialog() == true)
                {

                    Controller.Execute("update", new Vst.UpdateRequest
                    {
                        ObjectId = ((BsonData.IDocument)item).Id,
                        Action = Vst.UpdateActions.Delete,
                    });

                }
            };

            return new List<MyButton> { del };

        }

        protected virtual UIElement CreateFunctionButton(string url, string text, string css)
        {
            var btn = new MyButton
            {
                Url = url,
                Text = text,
                Margin = new Thickness(2),
                Padding = new Thickness(8, 5, 8, 5),
            };
            if (css != null) { btn.Css = css; }
            return btn;
        }
        protected virtual List<UIElement> CreateFunctions()
        {
            return new List<UIElement> {
                CreateFunctionButton(Controller.ControllerName + "/Create", "Thêm mới", null)
            };
        }
        protected override void LoadElements()
        {
            MainContent.AddRow(CreateCaption(null, CreateFunctions().ToArray()));
            MainContent.AddRow(CreateBody(typeof(TModel).Name));

            base.LoadElements();
        }
    }
}

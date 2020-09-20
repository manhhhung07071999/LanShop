using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.IO;
using System.Xml;
using System.Windows.Input;

namespace System.Windows.Controls
{
    public class MyGridViewRow : PanelElement<MyTableLayout>
    {
        public MyGridViewRow()
        {
            Background = Media.Brushes.White;
            Margin = new Thickness(0, 1, 0, 0);

            Css = "data-row";

            this.PreviewMouseLeftButtonDown += (s, e) => {
                if (e.ClickCount >= 2)
                {
                    Open?.Invoke(this.Tag);
                }
            };
        }

        public Action<object> Open;
        public override void SetContentValue(DependencyProperty property, object value)
        {
            foreach (UIElement e in this.Content.Children)
            {
                if (e is StyleElement)
                {
                    ((StyleElement)e).SetContentValue(property, value);
                }
            }

        }
    }
    public class MyGridView : MyTableLayout, IDisposable
    {
        MyTableLayout _captionView;
        ScrollViewer _contentView;

        System.Mvc.ITimer _timer;
        public void Dispose()
        {
            _timer?.Stop();
        }

        public MyGridView()
        {
            this.AddRow(_captionView = new MyTableLayout());
            Background = Media.Brushes.WhiteSmoke;
        }

        protected void LoadTemplate()
        {
            List<int> wCols = new List<int>();
            foreach (var p in _binding)
            {
                var border = new MyLabel
                {
                    Text = p.Value.Caption,
                    Css = "list-caption",
                };
                _captionView.AddColumn(border);
                wCols.Add(p.Value.Width);
            }

            _captionView.AddColumn(new MyLabel { Css = "list-caption" });
            _captionView.SetWidths(wCols.ToArray());
        }

        protected virtual void LoadItems()
        {
            if (_contentView != null)
            {
                this.Children.Remove(_contentView);
            }

            if (_itemsSource == null)
            {
                return;
            }

            int iRow = 0;
            var sp = new StackPanel { };

            this.AddRow(_contentView = new ScrollViewer
            {
                Content = sp,
            });

            var iter = ((System.Collections.IEnumerable)_itemsSource).GetEnumerator();
            if (iter.MoveNext() == false)
            {
                return;
            };

            _timer = Counter.Start(5, step => {

                var item = iter.Current;
                var type = item.GetType();
                var row = new MyGridViewRow
                {
                    Tag = item,
                    Open = (obj) => {
                        DoubleClick?.Invoke(obj);
                    },
                };

                row.PreviewMouseLeftButtonDown += (s, e) => {
                    var collection = ((StackPanel)(_contentView.Content)).Children;
                    //var row = (MyGridViewRow)sender;
                    var index = collection.IndexOf(row);

                    SetSelectedIndex(index, row);
                };

                foreach (ColumnDefinition col in _captionView.ColumnDefinitions)
                {
                    row.Content.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });
                }
                sp.Children.Add(row);
                ItemRowCreated?.Invoke(row, item);

                var iCol = 0;
                foreach (var p in _binding)
                {
                    var info = p.Value;
                    var prop = type.GetProperty(info.BindingName ?? p.Key);
                    if (prop != null)
                    {
                        var v = prop.GetValue(item);
                        if (v != null)
                        {
                            if (v is UIElement)
                            {
                                row.Content.Add(0, iCol, (UIElement)v);
                            }
                            else
                            {
                                if (info.FormatString != null)
                                {
                                    v = string.Format("{0:" + info.FormatString + "}", v);
                                }
                                var cell = new MyLabel
                                {
                                    Text = v.ToString(),
                                };
                                row.Content.Add(0, iCol, cell);
                            }
                        }
                    }
                    iCol++;
                }
                _rowCount = ++iRow;

                return iter.MoveNext();
            });
        }

        int _selectedIndex = -1;
        int _rowCount = 0;

        MyGridViewRow GetRowByIndex(int index)
        {
            var collection = ((StackPanel)(_contentView.Content)).Children;
            return index < 0 || index >= collection.Count ? null : (MyGridViewRow)collection[index];
        }
        void SetActiveRow(MyGridViewRow newRow, MyGridViewRow oldRow)
        {
            if (oldRow != null)
            {
                oldRow.Css = "data-row";
            }
            if (newRow != null)
            {
                newRow.Css = "data-row-active";
            }
        }
        void SetSelectedIndex(int index, MyGridViewRow newRow)
        {
            if (index < 0)
            {
                index = 0;
            }
            if (index >= _rowCount)
            {
                index = _rowCount - 1;
            }

            if (index != _selectedIndex)
            {
                var oldRow = GetRowByIndex(_selectedIndex);

                if (newRow == null)
                    newRow = GetRowByIndex(index);

                SetActiveRow(newRow, oldRow);
                _selectedIndex = index;
            }
        }
        void SetSelectedIndex(int index)
        {
            SetSelectedIndex(index, null);
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
            switch (e.Key)
            {
                case Key.Enter:
                    var r = GetRowByIndex(_selectedIndex);
                    if (r != null)
                    {
                        DoubleClick?.Invoke(r.Tag);
                    }
                    break;

                case Key.PageDown:
                    break;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Up:
                    SetSelectedIndex(_selectedIndex - 1);
                    break;

                case Key.Down:
                    SetSelectedIndex(_selectedIndex + 1);
                    break;
            }

            base.OnPreviewKeyDown(e);
       }

        object _itemsSource;
        public object ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                LoadItems();
            }
        }

        BindingInfoCollection _binding;
        public BindingInfoCollection Binding
        {
            get { return _binding; }
            set
            {
                if (_binding != value)
                {
                    _binding = value;
                    this.LoadTemplate();
                }
            }
        }

        public void Refresh()
        {
        }

        public event Action<object> DoubleClick;
        public event Action<MyGridViewRow, object> ItemRowCreated;
    }

    public class DataItemEventArgs : EventArgs
    {
        public object Item { get; set; }
    }

    public class MyControlBox : MyTableLayout
    {
        protected MyLabel _label;

       
    }
}

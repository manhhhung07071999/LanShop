using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Controls
{
    public class MySplitterPanel : MyTableLayout
    {
        Orientation _orientation = Orientation.Horizontal;

        const int splitterSize = 5;
        public Orientation Orientation
        {
            get { return _orientation; }
            set { SetOrientation(value); }
        }
        protected void SetOrientation(Orientation orientation)
        {
            _orientation = orientation;
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            if (orientation == Orientation.Horizontal)
            {
                for (int i = 0; i < 3; i++)
                {
                    RowDefinitions.Add(new RowDefinition());
                }
                RowDefinitions[1].Height = new GridLength(splitterSize);
            }
            else
            {
                RowDefinitions.Clear();
                for (int i = 0; i < 3; i++)
                {
                    ColumnDefinitions.Add(new ColumnDefinition());
                }
                ColumnDefinitions[1].Width = new GridLength(splitterSize);
            }
        }
        protected Border GetFirstPanel() { return (Border)Children[0]; }
        protected Border GetSecondPanel() { return (Border)Children[2]; }

        public UIElement FirstPanel
        {
            get { return GetFirstPanel().Child; }
            set { GetFirstPanel().Child = value; }
        }
        public UIElement SecondPanel
        {
            get { return GetSecondPanel().Child; }
            set { GetSecondPanel().Child = value; }
        }
        public void SetFirstPanelSize(double value, GridUnitType type)
        {
            if (_orientation == Orientation.Vertical)
            {
                var col = ColumnDefinitions[0];
                col.Width = new GridLength(value, type);
            }
            else
            {
                var row = RowDefinitions[0];
                row.Height = new GridLength(value, type);
            }
        }
        public void SetFirstPanelSize(double value)
        {
            SetFirstPanelSize(value, GridUnitType.Pixel);
        }
        public MySplitterPanel()
        {
            AddRange(
                new Border(),
                new GridSplitter
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Background = Media.Brushes.WhiteSmoke,
                },
                new Border());

            SetOrientation(Orientation.Vertical);
        }
    }
}

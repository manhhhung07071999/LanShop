using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//use
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.MatHang
{
    //class Edit : Editor<Models.MatHang>
    //{
    //    static BindingInfoCollection _bindingInfos;
    //    protected override BindingInfoCollection GetBindingInfo(string name)
    //    {
    //        if (_bindingInfos == null)
    //        {
    //            _bindingInfos = base.GetBindingInfo(name).Clone();
    //            _bindingInfos.Add("MoTa", new BindingInfo
    //            {
    //                Caption = "Mô tả",
    //                Input = "richText",
    //            });
    //        }

    //        return _bindingInfos;
    //    }

    //    protected override void InsertInput(IMyInput input, int index)
    //    {
    //        var cols = MainContent.ColumnDefinitions.Count;
    //        if (cols != 5)
    //        {
    //            MainContent.SetWidths(200, 10, 200, 10, 250);
    //            cols = MainContent.ColumnDefinitions.Count;
    //        }

    //        var element = (FrameworkElement)input;
    //        if (index == _bindingInfos.Count - 1)
    //        {
    //            MainContent.Add(0, cols - 1, element);
    //            element.SetValue(Grid.RowSpanProperty, 4);

    //            return;
    //        }

    //        switch (index)
    //        {
    //            case 0:
    //            case 2:
    //            case 3:
    //            case 5:
    //                MainContent.AddRow();
    //                break;
    //        }
    //        int r = MainContent.RowDefinitions.Count - 1;
    //        int c = 0;

    //        switch (index)
    //        {
    //            case 1:
    //            case 4:
    //            case 6:
    //                c = 2;
    //                break;

    //            case 2:
    //                element.SetValue(Grid.ColumnSpanProperty, cols - 2);
    //                break;
    //        }

    //        MainContent.Add(r, c, element);
    //    }
    //}

    //class Create : Edit { }
}

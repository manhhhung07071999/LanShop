using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views
{
    enum Orientations { Vertical, Horizontal };
    class ControlBox : MyTableLayout
    {
        Orientations _orientation;
        public Orientations Orientation
        {
            get { return _orientation; }
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    this.OnOrientationChanged();
                }
            }
        }
        protected virtual void OnOrientationChanged() { }

        List<IMyInput> _inputs = new List<IMyInput>();
        public List<IMyInput> Inputs { get { return _inputs; } }

        public IMyInput this[string name]
        {
            get
            {
                foreach (var inp in Inputs)
                {
                    if (inp.BindingName == name)
                    {
                        return inp;
                    }
                }
                return null;
            }
        }

        BindingInfoCollection _binding;
        public BindingInfoCollection Binding
        {
            get { return _binding; }
            set
            {
                _binding = value;
                foreach (var p in Binding)
                {
                    var info = p.Value;
                    var input = CreateInput(p.Key, info);

                    if (input == null) { continue; }

                    _inputs.Add(input);

                    input.Caption = info.Caption;
                    input.BindingName = info.BindingName ?? p.Key;
                    input.Required = info.AllowNull == false;
                }

                // Chi dih layout
                if (LayoutInputs != null)
                {
                    LayoutInputs.Invoke(this);
                    return;
                }

                // Truong hop khong chi dinh Layout
                if (_orientation == Orientations.Vertical)
                {
                    foreach (var e in Inputs)
                    {
                        this.AddRow(e.CreateLabel());
                        this.AddRow((UIElement)e);
                    }
                }
                else
                {
                    this.AddColumn();
                    this.AddColumn();
                    foreach (var e in Inputs)
                    {
                        int r = this.AddRow();
                        this.Add(r, 0, e.CreateLabel());

                        var input = (StyleElement)e;
                        input.Style.Margin = new Bound(10, 2);
                        this.Add(r, 1, input);
                    }
                }

            }
        }

        object _value;
        public object Value
        {
            get
            {
                var res = true;

                var type = _value.GetType();
                foreach (var input in _inputs)
                {
                    var v = input.Value;
                    if (v == null && input.Required)
                    {
                        res = false;
                        //input.Error = input.Caption + ' ' + GetTextByCode("input-value-required");
                        continue;
                    }

                    var p = type.GetProperty(input.BindingName);
                    if (p != null)
                    {
                        p.SetValue(_value, v);
                    }
                }
                return res ? _value : null;
            }
            set
            {
                _value = value;

                var type = value.GetType();
                foreach (var input in _inputs)
                {
                    var prop = type.GetProperty(input.BindingName);

                    if (prop == null) continue;
                    var v = prop.GetValue(value);
                    //if (v != null && info.FormatString != null)
                    //{
                    //    v = string.Format(info.FormatString, v);
                    //}
                    input.Value = v;
                }
            }
        }

        public Action<ControlBox> LayoutInputs;

        public static IMyInput CreateInput(string name, BindingInfo info)
        {
            if (info.Input != null)
            {
                if (info.Input == "none") return null;
                switch (info.Input[0])
                {
                    case 'i': return new MyIntegerBox();
                    case 'd': return new MyDateBox();
                    case 'p': return new MyPasswordBox();
                    case 'c':
                        if (info.Input[1] == 'o')
                            return new MyComboBox();
                        return new MyCheckBox();

                    case 'r': return new MyRichTextBox();
                }
            }
            return new MyTextBox();
        }

    }
}

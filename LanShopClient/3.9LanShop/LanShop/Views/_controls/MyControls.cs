using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;

namespace System.Windows.Controls
{
    public interface IMyInput
    {
        object Value { get; set; }
        string BindingName { get; set; }
        string Error { get; set; }
        string Caption { get; set; }
        bool Required { get; set; }
        bool Disabled { get; set; }
        double Width { get; set; }

        UIElement CreateLabel();
        void Focus();
    }
    public abstract class MyInput<T> : PanelElement<Grid>, IMyInput
        where T : Control, new()
    {
        public string BindingName { get; set; }
        public string Error { get; set; }
        public bool Required { get; set; }

        public bool Disabled
        {
            get { return Input.IsEnabled == false; }
            set { Input.IsEnabled = !value; }
        }
        new public virtual void Focus() { Input.Focus(); }

        protected TextElement<T> _inputContainer;

        public virtual string Caption
        {
            get; set;
        }
        public UIElement CreateLabel()
        {
            return new MyLabel {
                Text = Caption,
                Css = "input-caption",
            };
        }

        public MyInput()
        {
            this.Add(_inputContainer = new TextElement<T> {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Css = "input",
                Padding = new Thickness(10),
            });

            Input.BorderThickness = new Thickness(0);
            this.PreviewMouseDown += (s, e) => Input.Focus();
        }

        public T Input => _inputContainer.Content;

        public abstract object Value { get; set; }

        MyButton _button;
        public MyButton Button
        {
            get
            {
                if (_button == null)
                {
                    var mg = new Thickness(3);
                    mg.Top = _inputContainer.Margin.Top + mg.Bottom;

                    _button = new MyButton {
                        Margin = mg,
                    };
                    _button.Style.HAlign = LayoutOptions.Far;

                    this.Add(_button);
                }
                return _button;
            }
        }
    }

    class MyTextBox : MyInput<TextBox>
    {
        public MyTextBox()
        {
            Input.GotFocus += (s, e) => Input.SelectAll();
        }
        public override object Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Input.Text))
                    return null;
                return Input.Text;
            }

            set
            {
                Input.Text = value?.ToString();
            }
        }
    }

    class MyRichTextBox : MyInput<RichTextBox>
    {
        public MyRichTextBox()
        {
            Input.SetValue(Paragraph.LineHeightProperty, 1.0);
        }
        public override object Value
        {
            get { return new TextRange(Input.Document.ContentStart, Input.Document.ContentEnd).Text.Trim(); }
            set
            {
                var v = value ?? "";

                var parag = new Paragraph();
                parag.Inlines.Add(v.ToString());

                Input.Document.Blocks.Clear();
                Input.Document.Blocks.Add(parag);
            }
        }
    }

    class MyDateBox : MyTextBox
    {
        public override object Value
        {
            get
            {
                var dv = new int[] { 0, DateTime.Today.Month, DateTime.Today.Year };
                int i = 0;
                foreach (var s in Input.Text.Split(' ', '/', '.', '-'))
                {
                    int d;
                    if (int.TryParse(s, out d)) { dv[i] = d; }

                    if (++i >= dv.Length) { break; }
                }

                if (dv[2] < 100) dv[2] += 2000;
                return new DateTime(dv[2], dv[1], dv[0]);
            }

            set
            {
                base.Value = string.Format("{0:dd/MM/yyyy}", value);
            }
        }
    }

    class MyPasswordBox : MyInput<PasswordBox>
    {
        public override object Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Input.Password))
                    return null;
                return Input.Password;
            }

            set
            {
                Input.Password = (string)value;
                //SetLabelPosition();
            }
        }
    }

    class MyComboBox : MyInput<ComboBox>
    {
        public MyComboBox()
        {
        }

        public override object Value
        {
            get
            {
                return Input.SelectedValue;
            }

            set
            {
                Input.SelectedValue = value;
            }
        }
        //protected override void SetLabelPosition()
        //{
        //    //base.SetLabelPosition();
        //}
    }

    class MyIntegerBox : MyTextBox
    {
        public MyIntegerBox()
        {
            Input.Text = "0";
            Input.PreviewKeyDown += (s, e) =>
            {
                if (!IsKeyValid(e.Key))
                {
                    e.Handled = true;
                }
            };
        }
        protected virtual bool IsKeyValid(Key key)
        {
            if (key == Key.OemPlus || key == Key.OemMinus)
            {
                return this.Input.Text == string.Empty;
            }

            return key == Key.Enter 
                || key == Key.Delete 
                || key == Key.Back 
                || key == Key.Left
                || key == Key.Right
                || key == Key.Tab
                || (key >= Key.D0 && key <= Key.D9);
        }

        public override object Value
        {
            get
            {
                var s = Input.Text;
                if (string.IsNullOrWhiteSpace(s))
                    return 0;

                return int.Parse(Input.Text);
            }

            set {  base.Value = value; }
        }
    }

    class MyNumberBox : MyIntegerBox
    {
        protected override bool IsKeyValid(Key key)
        {
            if (key == Key.Decimal)
            {
                foreach (char c in this.Input.Text)
                    if (c == '.')
                        return false;
                return true;
            }
            return base.IsKeyValid(key);
        }

        public override object Value
        {
            get
            {
                long a = 0, b = 0;
                bool m = false;
                foreach (char c in this.Input.Text)
                {
                    switch (c)
                    {
                        case '-': m = true; continue;
                        case '+': continue;
                        case '.': b = 1; continue;
                    }
                    a = (a << 1) + (a << 3) + (c & 15);
                    if (b > 0) b = (b << 1) + (b << 3);
                }
                if (b > 1) return (double)a / b;

                return a;
            }
            set => base.Value = value;
        }
    }

    class MyCheckBox : MyInput<CheckBox>
    {
        Border _ticker;
        public MyCheckBox()
        {
            this.MaxHeight = 25;
            this.MaxWidth = 25;
            this.Style.HAlign = LayoutOptions.Near;
            this.Style.VAlign = LayoutOptions.Center;
            this.Margin = new Thickness(0, 10, 0, 10);

            var ticker = new Border {
                RenderTransform = new RotateTransform(-45),
                Width = 20,
                Height = 10,
                BorderThickness = new Thickness(2, 0, 0, 1),
                BorderBrush = Brushes.Green,
                Margin = new Thickness(0, 8, 0, 0),
                Visibility = Visibility.Hidden,
            };
            this.Add(_ticker = ticker);
            this.Input.Checked += (s, e) => {
                ShowTicker();
            };
            this.Input.Unchecked += (s, e) => {
                ShowTicker();
            };
            this.Background = Brushes.White;
            this.PreviewMouseUp += (s, e) => {
                Input.IsChecked ^= true;
                ShowTicker();
            };
        }

        void ShowTicker()
        {
            _ticker.Visibility = (Input.IsChecked == true ? Visibility.Visible : Visibility.Hidden);
        }
        public override object Value
        {
            get => Input.IsChecked;
            set => Input.IsChecked = (bool)value;
        }
    }
}

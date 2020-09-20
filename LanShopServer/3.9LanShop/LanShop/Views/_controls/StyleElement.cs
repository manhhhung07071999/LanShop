using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace System.Windows
{
    public enum LayoutOptions { Near, Center, Far, Stretch };
}
namespace System.Windows.Controls
{ 
    public class Style
    {
        StyleElement _control;
        public Style(StyleElement control)
        {
            _control = control;
        }
        public Rgb BackgroundColor
        {
            get { return (Brush)_control.GetValue(Border.BackgroundProperty); }
            set { _control.SetValue(Border.BackgroundProperty, (Brush)value); }
        }
        public Rgb BorderColor
        {
            get { return (Brush)_control.GetValue(Border.BorderBrushProperty); }
            set { _control.SetValue(Border.BorderBrushProperty, (Brush)value); }
        }
        public Bound Margin
        {
            get { return (Thickness)_control.GetValue(Border.MarginProperty); }
            set { _control.SetValue(Border.MarginProperty, (Thickness)value); }
        }
        public Bound Padding
        {
            get { return (Thickness)_control.GetValue(Border.PaddingProperty); }
            set { _control.SetValue(Border.PaddingProperty, (Thickness)value); }
        }

        public LayoutOptions HAlign
        {
            get { return (LayoutOptions)_control.GetValue(Border.HorizontalAlignmentProperty); }
            set { _control.SetValue(Border.HorizontalAlignmentProperty, (HorizontalAlignment)value); }
        }
        public LayoutOptions VAlign
        {
            get { return (LayoutOptions)_control.GetValue(Border.VerticalAlignmentProperty); }
            set { _control.SetValue(Border.VerticalAlignmentProperty, (VerticalAlignment)value); }
        }

        public Rgb TextColor
        {
            get { return (Brush)_control.GetContentValue(Control.ForegroundProperty); }
            set { _control.SetContentValue(Control.ForegroundProperty, (Brush)value); }
        }
        public double FontSize
        {
            get { return (double)_control.GetContentValue(Control.FontSizeProperty); }
            set { _control.SetContentValue(Control.FontSizeProperty, value); }
        }
        public int FontWeight
        {
            get { return ((FontWeight)_control.GetContentValue(Control.FontWeightProperty)).ToOpenTypeWeight(); }
            set
            {
                _control.SetContentValue(Control.FontWeightProperty,
                    System.Windows.FontWeight.FromOpenTypeWeight(value));
            }
        }

    }
    public abstract class StyleElement : Border
    {

        public StyleElement()
        {
        }

        Style _style;
        new public Style Style
        {
            get
            {
                if (_style == null)
                {
                    _style = new Style(this);
                }
                return _style;
            }
        }

        string _css;
        StyleSheetList _styles;
        public string Css
        {
            get { return _css; }
            set
            {
                if (_css == value) { return; }

                _css = value;
                if (_styles != null)
                {
                    foreach (var e in _styles)
                    {
                        e.Unapply(this);
                    }
                }
                _styles = _css;
                _styles.Apply(this);
            }
        }

        public abstract UIElement GetContent();
        public virtual object GetContentValue(DependencyProperty property)
        {
            return GetContent().GetValue(property);
        }
        public virtual void SetContentValue(DependencyProperty property, object value)
        {
            GetContent().SetValue(property, value);
        }

        public virtual void Refresh() { }
    }
    public class StyleElement<T> : StyleElement
        where T: UIElement, new()
    {
        public StyleElement()
        {
            Child = new T();
        }

        public override UIElement GetContent()
        {
            return (T)Child;
        }
        public T Content
        {
            get { return (T)Child; }
            set {  Child = value; }
        }
    }
    public class PanelElement<T> : StyleElement<T>
        where T: Panel, new()
    {
        public PanelElement()
        {
            Child = new T();
        }

        public void AddRange(params UIElement[] items)
        {
            var children = ((Panel)Child).Children;
            foreach (var e in items) { children.Add(e); }
        }

        public void Add(UIElement item)
        {
            var children = ((Panel)Child).Children;
            children.Add(item);
        }
        public void Clear()
        {
            Content.Children.Clear();
        }
    }

    public class TextElement<T> : StyleElement<T>
        where T: Control, new()
    {

        new public bool IsFocused => Content.IsFocused;
        public virtual string Text { get; set; }
       
    }

    public class MyLabel : TextElement<Label>
    {
        public MyLabel()
        {
            Content.HorizontalAlignment = HorizontalAlignment.Center;
            Content.VerticalAlignment = VerticalAlignment.Center;
        }
        public override string Text
        {
            get { return (string)Content.GetValue(Label.ContentProperty); }
            set {  Content.SetValue(Label.ContentProperty, value); }
        }
    }
}

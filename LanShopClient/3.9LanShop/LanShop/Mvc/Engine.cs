using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using System.Windows;
using System.Windows.Controls;

namespace System
{
    public interface IUpdateView
    {
        void UpdateView(object content);
    }
    public interface IModal
    {
        bool? ShowDialog();
    }
}

namespace LanShop
{
    public interface IClientRenderer
    {
        void ProcessResponse(object value);
    }

    public class MyApp : System.Mvc.Engine
    {
        static IClientRenderer _rpEngine { get; set; }
        public static void ExecResponse(object value)
        {
            _rpEngine.ProcessResponse(value);
        }

        static Counter _clock;
        public static Counter SystemClock
        {
            get
            {
                if (_clock == null)
                {
                    _clock = new Counter();
                }
                return _clock;
            }
        }

        static ActionResult _asyncResult;
        static IUpdateView _mainContent;

        static void SetMainContent(System.Mvc.IView view)
        {
            var render = ((System.Mvc.IRenderer)view);
            var content = render?.GetResult();

            if (content != null)
            {
                if (content is IModal)
                {
                    ((IModal)content).ShowDialog();
                    return;
                }

                _rpEngine = view as IClientRenderer;
                _mainContent.UpdateView(content);
            }
        }
        public static void Start(object mainContent)
        {
            _mainContent = (IUpdateView)mainContent;
            Register(mainContent, result => {
                var view = result.View;
                if (view != null)
                {
                    var async = view as System.Mvc.IAsyncView;
                    if (async != null)
                    {
                        _asyncResult = result;
                        return;
                    }
                    SetMainContent(view);
                }
            });

            SystemClock.Milliseconds += (ms) => {
                if (_asyncResult != null && ms > 250)
                {
                    var view = _asyncResult.View;
                    view.Render(_asyncResult.Controller);
                    SetMainContent(view);

                    _asyncResult = null;
                }
            };
        }

        public static string ApplicationPath => System.IO.Directory.GetCurrentDirectory();
        public static string MapPath(string path)
        {
            return ApplicationPath + '/' + path;
        }

        public static Browser Browser { get; set; }
    }

    public class Browser : System.Windows.Window
    {
        public Browser()
        {
            StyleSheetCollection.Include(MyApp.MapPath("/contents/default.json"));
            StyleSheetCollection.Include(MyApp.MapPath("/contents/test.json"));
            BindingInfoCollection.Include(MyApp.MapPath("/views/_binding/autoform.json"));

            var menu = Vst.Json.Read<List<MyMenuItemInfo>>(MyApp.MapPath("/views/_binding/menu.json"));

            var layout = new MyLayout(MyLayoutOptions.NavMenu)
            {
                ApplicationText = "Server Manager",
            };
            this.Content = layout;

            layout.NavMenu.ItemsSource = menu;

            MyApp.Browser = this;
            MyApp.Start(layout);

            MyApp.Execute("home");

            this.PreviewKeyUp += (s, e) => {

            };

        }
    }
}

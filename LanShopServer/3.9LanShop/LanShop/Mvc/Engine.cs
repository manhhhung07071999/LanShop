using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using System.Windows;
using System.Threading;
using System.ComponentModel;

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
    public class MyApp : System.Mvc.Engine
    {
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

        static void SetMainContent(IView view)
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

            _threads = new Stack<Thread>();
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
        public static void Stop()
        {
            while (_threads.Count > 0)
            {
                var th = _threads.Pop();
                if (th.IsAlive)
                {
                    th.Abort();
                }
            }
        }

        public static string ApplicationPath => System.IO.Directory.GetCurrentDirectory();
        public static string MapPath(string path)
        {
            return ApplicationPath + '\\' + path;
        }

        public static Browser Browser { get; set; }

        static Stack<Thread> _threads;
        public static Thread BeginInvoke(Action action)
        {
            while (_threads.Count > 0)
            {
                if (_threads.Peek().IsAlive) break;

                _threads.Pop();
            }
            var th = new Thread(new ThreadStart(() => action()));
            _threads.Push(th);

            th.Start();
            return th;
        }
    }

    public class Browser : System.Windows.Window
    {
        public Browser()
        {
            StyleSheetCollection.Include(MyApp.MapPath("contents/default.json"));
            StyleSheetCollection.Include(MyApp.MapPath("contents/test.json"));
            BindingInfoCollection.Include(MyApp.MapPath("views/_binding/autoform.json"));

            var menu = Vst.Json.Read<List<MyMenuItemInfo>>(MyApp.MapPath("/views/_binding/menu.json"));

            var layout = new System.Windows.Controls.MyLayout
            {
                ApplicationText = "LAN SHOP Server",
            };
            this.Content = layout;

            layout.TopMenu.ItemsSource = menu;
            layout.TopMenu.ItemActivated += (e) => {
                layout.NavMenu.ItemsSource = e.Data.Childs;
            };

            layout.NavMenu.ItemsSource = layout.TopMenu[1].Data.Childs;

            MyApp.Browser = this;
            MyApp.Start(layout);

            MyApp.Execute("home");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            MyApp.Stop();
        }
    }
}

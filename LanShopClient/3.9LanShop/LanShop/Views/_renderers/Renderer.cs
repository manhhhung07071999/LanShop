using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Dic = System.Collections.Generic.Dictionary<string, string>;

namespace LanShop.Views
{
    class Dictionary
    {
        Dic _data;
        public void Load(string name)
        {
            _data = Vst.Json.Read<Dic>(MyApp.MapPath("views/_dictionary/" + name + ".json"));
        }
        public string GetString(string key)
        {
            string s;
            _data.TryGetValue(key, out s);

            return s ?? key;
        }
    }

    class Renderer<TView, TModel> : System.Mvc.Renderer<TView, TModel>, IClientRenderer
        where TView : Panel, new()
    {

        public virtual void ProcessResponse(object value)
        {

        }

        static Dictionary _dic;
        protected string GetTextByCode(string code)
        {
            if (_dic == null)
            {
                _dic = new Dictionary();
                _dic.Load("viet");
            }
            return _dic.GetString(code);
        }
        protected override void RenderCore(Controller controller, ViewDataDictionary viewData, TModel model, TView mainContent)
        {
            LoadElements();
        }
        protected virtual void InsertElement(UIElement element)
        {
            MainContent.Children.Add(element);
        }
        protected virtual void LoadElements()
        {
            var methods = this.GetType().GetRuntimeMethods();
            foreach (var method in methods)
            {
                if (method.Name[0] == '_')
                {
                    InsertElement((UIElement)method.Invoke(this, new object[] { }));
                }
            }
        }
    }

    class Renderer<TModel> : Renderer<MyTableLayout, TModel>
    {
        protected override void InsertElement(UIElement element)
        {
            MainContent.AddRow(element);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views
{
    class Editor<TModel> : Renderer<ControlBox, Vst.UpdateRequest>
        where TModel : new()
    {

        /// <summary>
        /// Hàm lấy template (mặc định là tên Model)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual BindingInfoCollection GetBindingInfo(string name)
        {
            BindingInfoCollection infos = name;
            return infos;
        }

        /// <summary>
        /// Hàm sinh input
        /// </summary>
        /// <param name="name"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual IMyInput CreateInput(string name, BindingInfo info)
        {
            return ControlBox.CreateInput(name, info);
        }

        /// <summary>
        /// Hàm sinh các input
        /// </summary>
        /// <param name="infos"></param>
        protected virtual void RenderInputs(BindingInfoCollection infos)
        {
            MainContent.Binding = infos;
            MainContent.Value = Model?.Value ?? new TModel();
        }

        protected override void LoadElements()
        {
            var infos = GetBindingInfo(typeof(TModel).Name);
            RenderInputs(infos);
        }

        protected virtual void SetInputPosition(IMyInput input, int r, int c)
        {
            SetInputPosition(input, r, c, 0, 0);
        }
        protected virtual void SetInputPosition(IMyInput input, int r, int c, int w, int h)
        {
            var e = (UIElement)input;
            if (w > 1)
            {
                e.SetValue(Grid.ColumnSpanProperty, w);
            }
            if (h > 1)
            {
                e.SetValue(Grid.RowSpanProperty, h);
            }

            MainContent.Add(r, c, input.CreateLabel());
            MainContent.Add(r + 1, c, e);
        }

        /// <summary>
        /// Hàm cập nhật dữ liệu từ các input vào Model
        /// </summary>
        /// <returns></returns>
        protected virtual bool UpdateModel(string actionName)
        {
            var value = MainContent.Value;
            if (value != null)
            {
                Model.Value = value;
                Controller.Execute(actionName ?? "update", Model);

                return true;
            }
            return false;
        }
        public bool UpdateModel() { return UpdateModel(null); }

        protected virtual Dialog CreateDialog(string accept, string cancel)
        {
            return new Dialog(null, accept, cancel);
        }
        public override object GetResult()
        {
            MainContent.Margin = new Thickness(20);

            var dialog = CreateDialog("OK", "Cancel");
            dialog.Body = MainContent;

            dialog.UpdateCompleted = UpdateModel;

            var ts = new System.Threading.ThreadStart(() =>
            {
                System.Threading.Thread.Sleep(100);
                dialog.Dispatcher.InvokeAsync(() => {
                    foreach (var e in MainContent.Inputs)
                    {
                        if (e.Disabled == false)
                        {
                            e.Focus();
                            return;
                        }
                    }
                });
            });
            new System.Threading.Thread(ts).Start();
            return dialog;
        }
    }
}

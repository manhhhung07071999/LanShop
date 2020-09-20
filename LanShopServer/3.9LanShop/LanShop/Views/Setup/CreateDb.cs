using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LanShop.Views.Setup
{
    class DriveBox : MyTableLayout
    {
        Grid CreateDriveItem(string name)
        {
            var img = new Image
            {
                Source = new ImageRenderer("folder").ToImage(),
                Width = 75,
                Height = 60,
                Margin = new Thickness(20)
            };
            var btn = new MyButtonBase
            {
                Text = name,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Css = "disk",
            };

            btn.Click += (e) => {
                OnDriverSelected?.Invoke(btn.Text);
            };

            return new Grid {
                Margin = new Thickness(2),
                Children = { img, btn }
            };
        }
        public DriveBox(IEnumerable<string> drives)
        {
            this.Margin = new Thickness(40);

            this.SetWidths(100, 100, 100);
            int r = this.AddRow();
            int c = 0;

            foreach (var dir in drives)
            {
                var item = CreateDriveItem(dir);
                this.Add(r, c++, item);

                if (c == this.ColumnDefinitions.Count)
                {
                    r = this.AddRow();
                }
            }
        }

        public Action<string> OnDriverSelected;
    }

    class CreateDb : Renderer<List<string>>
    {
        public override object GetResult()
        {
            var dlg = new Dialog(null, null, "Close");
            var layout = new MyTableLayout();

            layout.AddRow(new MyLabel {
                Text = GetTextByCode("db-0"),
            });
            layout.AddRow(new DriveBox(Model) {

                OnDriverSelected = (s) => {
                    dlg.DialogResult = true;
                    Controller.Execute(Controller.RequestContext.ActionName, s);
                },
            });

            dlg.Body = layout;
            if (dlg.ShowDialog() == false)
            {
                Controller.Execute(Controller.RequestContext.ActionName, false);
            }

            return null;
        }
    }

    class RunClone : Renderer<Models.Data.CloneModel>
    {
        public override object GetResult()
        {
            var dlg = new Dialog(null, null, "Cancel")
            {
                Body = MainContent,
            };

            var file = new MyLabel {
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Coping Files"
            };
            var prog = new ProgressBar {
                Maximum = Model.Source.Length,
                Margin = new Thickness(20, 0, 20, 20),
            };

            MainContent.AddRow(file);
            MainContent.AddRow(prog);

            int i = 0;
            var th = MyApp.BeginInvoke(() => {
                System.Threading.Thread.Sleep(500);
                while (i < Model.Source.Length)
                {
                    var s = Model.Source[i];
                    var f = s.Substring(3);

                    dlg.Dispatcher.InvokeAsync(() => {
                        prog.Value = ++i;
                        file.Text = f;
                        System.IO.File.Copy(s, Model.Destination + f);
                    });
                    System.Threading.Thread.Sleep(200);
                }

                MyApp.Browser.Dispatcher.InvokeAsync(() => {
                    dlg.DialogResult = true;
                    Controller.Execute("CloneCompleted");
                });
            });

            dlg.Closing += (s, e) => {
                if (th.IsAlive)
                {
                    th.Abort();
                }
            };

            return dlg;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace System.Windows.Controls
{
    public class MyFolderBox : MyGridView
    {
        protected static System.Windows.Media.ImageSource _folder, _file, _up;
        static DirectoryInfo _current;

        class FileItem
        {
            public FileSystemInfo FileInfo;

            public virtual string Name
            {
                get { return FileInfo.Name; }
            }
            public virtual Image Icon => new Image
            {
                Source = _file,
                Width = 20,
                Height = 20,
                Margin = new System.Windows.Thickness(4, 0, 0, 0),
            };
        }

        class Folder : FileItem
        {
            public override Image Icon => new Image
            {
                Source = _folder,
                Width = 28,
                Height = 20,
            };
        }

        class Up : Folder
        {
            public override Image Icon => new Image
            {
                Source = _up,
                Width = 20,
                Height = 20,
                Margin = new System.Windows.Thickness(4, 0, 0, 0),
            };
            public override string Name => "..";
        }

        public DirectoryInfo SelectedFolder => _current;

        void OpenDirectory(string dir)
        {
            var di = new DirectoryInfo(dir);
            OpenDirectory(di);
        }
        void OpenDirectory(DirectoryInfo di)
        {
            _current = di;
            var lst = new List<FileItem> { new Up { FileInfo = _current.Parent } };
            foreach (var si in di.GetDirectories())
            {
                lst.Add(new Folder { FileInfo = si });
            }

            if (FileSelected != null)
            {
                foreach (var fi in di.GetFiles())
                {
                    lst.Add(new FileItem { FileInfo = fi });
                }
            }
            this.ItemsSource = lst;

            FolderOpened?.Invoke();
        }

        public MyFolderBox() : this(null) { }
        public MyFolderBox(string path)
        {
            if (_folder == null)
            {
                _folder = new System.Windows.ImageRenderer("folder").ToImage();
                _file = new System.Windows.ImageRenderer("file").ToImage();
                _up = new System.Windows.ImageRenderer("up-left").ToImage();
            }

            this.Binding = "File";
            if (path == null)
            {
                path = _current?.FullName ?? Directory.GetCurrentDirectory();
            }

            this.OpenDirectory(path);
            this.DoubleClick += (item) =>
            {
                var info = (FileItem)item;
                if (info is Folder)
                {
                    OpenDirectory((DirectoryInfo)info.FileInfo);
                    return;
                }

                FileSelected?.Invoke((FileInfo)info.FileInfo);
            };
        }
        public event Action FolderOpened;
        public event Action<FileInfo> FileSelected;
    }
}

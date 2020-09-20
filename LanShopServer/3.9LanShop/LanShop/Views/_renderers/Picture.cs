using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanShop;

namespace System.Windows
{
    using IO;
    using Media;
    using Media.Imaging;
    using Pic = System.Drawing.Bitmap;

    public class PictureRenderer
    {
        public static BitmapImage Convert(Pic src)
        {
            MemoryStream ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        protected Pic _bitmap;
        public static explicit operator Media.ImageSource(PictureRenderer renderer)
        {
            return Convert(renderer._bitmap);
        }

        public PictureRenderer(string filename)
        {
            _bitmap = new Pic(filename);
        }
    }
    public class ImageRenderer : PictureRenderer
    {
        public ImageRenderer(string section, string key)
            : base(MyApp.MapPath(section + "\\" + key + ".png"))
        {
        }
        public ImageRenderer(string key) : this("Images", key) { }

        public ImageSource ToImage()
        {
            return Convert(_bitmap);
        }
        public ImageSource ToImage(Color c) { return ToImage(c.R, c.G, c.B); }
        public ImageSource ToImage(byte r, byte g, byte b)
        {
            var bmp = new Pic(_bitmap.Width, _bitmap.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var p = _bitmap.GetPixel(x, y);
                    if (p.A != 0)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(p.A, r, g, b));
                    }
                }
            }
            return Convert(bmp);
        }

        public ImageSource ToImage(int color)
        {
            return ToImage((byte)(color >> 16), (byte)((color >> 8) & 255), (byte)(color & 255));
        }
    }
}

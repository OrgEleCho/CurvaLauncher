using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers.Wpf;

namespace CurvaLauncher.Utilities
{
    public static partial class ImageUtils
    {
        public static ImageSource EmptyImage => new DrawingImage();

        public static ImageSource? GetEmbededIconImage(string filename, int iconSize)
        {
            if (!File.Exists(filename))
                return null;

            try
            {
                return FileIconHelper.GetEmbededIconImage(filename, iconSize);
            }
            catch
            {
                return null;
            }
        }

        public static ImageSource? GetAssociatedIconImage(string filename, int iconSize)
        {
            if (!File.Exists(filename))
                return null;

            try
            {
                return FileIconHelper.GetAssociatedIconImage(filename, iconSize > 32);
            }
            catch
            {
                return null;
            }
        }

        public static ImageSource? GetFileIcon(string filename, int iconSize)
        {
            if (!File.Exists(filename))
                return null;

            try
            {
                return
                    FileIconHelper.GetEmbededIconImage(filename, iconSize) ??
                    FileIconHelper.GetAssociatedIconImage(filename, iconSize > 32);
            }
            catch
            {
                return null;
            }
        }

        public static ImageSource? CreateFromSvg(string svg)
        {
            try
            {
                using StringReader stringReader = new StringReader(svg);
                WpfDrawingSettings settings = new WpfDrawingSettings();

                SharpVectors.Converters.FileSvgReader reader = new SharpVectors.Converters.FileSvgReader(settings);

                var drawing = reader.Read(stringReader);

                return new DrawingImage(drawing);
            }
            catch
            {
                return null;
            }
        }
    }
}

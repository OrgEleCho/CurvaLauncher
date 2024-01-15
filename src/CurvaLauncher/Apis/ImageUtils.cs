using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CurvaLauncher.Apis;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers.Wpf;

namespace CurvaLauncher.Apis
{
    public partial class ImageApi : IImageApi
    {
        private ImageApi() { }

        public static ImageApi Instance { get; } = new();

        public ImageSource EmptyImage => new DrawingImage();

        public ImageSource? GetEmbededIconImage(string filename, int iconSize, int? iconIndex)
        {
            if (!File.Exists(filename))
                return null;

            try
            {
                return FileIconHelper.GetEmbededIconImage(filename, iconSize, iconIndex);
            }
            catch
            {
                return null;
            }
        }

        public ImageSource? GetAssociatedIconImage(string filename, int iconSize)
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

        public ImageSource? GetFileIcon(string filename, int iconSize)
        {
            if (!File.Exists(filename))
                return null;

            try
            {
                return
                    FileIconHelper.GetEmbededIconImage(filename, iconSize, 1) ??
                    FileIconHelper.GetAssociatedIconImage(filename, iconSize > 32);
            }
            catch
            {
                return null;
            }
        }

        public ImageSource? CreateFromSvg(string svg)
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

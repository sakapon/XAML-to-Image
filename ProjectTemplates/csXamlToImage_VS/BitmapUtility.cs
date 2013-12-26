﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace $safeprojectname$
{
    public static class BitmapUtility
    {
        public static BitmapSource CreateImage(FrameworkElement element)
        {
            var bitmap = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, PixelFormats.Default);
            bitmap.Render(element);
            return bitmap;
        }

        public static BitmapSource CreateImage(Visual visual, Size size)
        {
            var bitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);
            return bitmap;
        }

        public static BitmapSource ZoomImage(BitmapSource source, Size size)
        {
            var visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                context.DrawImage(source, new Rect(new Point(), size));
            }

            var bitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Default);
            bitmap.Render(visual);
            return bitmap;
        }

        public static void SaveImage(string filePath, BitmapSource bitmap)
        {
            var fullPath = Path.GetFullPath(filePath);

            var encoder = CreateBitmapEncoder(fullPath);
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = File.Create(fullPath))
            {
                encoder.Save(stream);
            }
        }

        static BitmapEncoder CreateBitmapEncoder(string filePath)
        {
            switch (Path.GetExtension(filePath).ToLowerInvariant())
            {
                case ".bmp":
                    return new BmpBitmapEncoder();
                case ".gif":
                    return new GifBitmapEncoder();
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                    return new JpegBitmapEncoder();
                case ".png":
                    return new PngBitmapEncoder();
                case ".tiff":
                case ".tif":
                    return new TiffBitmapEncoder();
                case ".wdp":
                case ".hdp":
                    return new WmpBitmapEncoder();
                default:
                    throw new ArgumentException("Can not encode bitmaps for the specified file extension.", "filePath");
            }
        }
    }
}

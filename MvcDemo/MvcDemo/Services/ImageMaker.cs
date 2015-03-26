using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MvcDemo.Services {
    public interface IImageMaker {
        byte[] MakeThumbnail(byte[] bytes);
    }
    public class ImageMaker : IImageMaker {
        static readonly Size ThumbnailMaximumSize = new Size(500, 700);

        public byte[] MakeThumbnail(byte[] bytes) {
            using (var ms = new MemoryStream(bytes)) {
                using (var originalImage = Image.FromStream(ms)) {
                    var newSize = CalculateNewSize(originalImage);
                    var thumbnail = new Bitmap(originalImage, newSize);
                    var outStream = new MemoryStream();
                    thumbnail.Save(outStream, ImageFormat.Png);

                    return outStream.ToArray();
                }
            }
        }

        static Size CalculateNewSize(Image originalImage) {
//            var originalWidth = originalImage.Width;
//            var originalHeight = originalImage.Height;
            var original = originalImage.Size;

            if (original.Width < ThumbnailMaximumSize.Width
                    && original.Height < ThumbnailMaximumSize.Height) {
                return original;
            }

            var newSize = new Size();
            var aspect = ((double) original.Width) / original.Height;
            if (aspect > 0) {
                // lanscape
                newSize.Width = ThumbnailMaximumSize.Width;
                newSize.Height = (int) (newSize.Width / aspect);
            } else {
                newSize.Height = ThumbnailMaximumSize.Height;
                newSize.Width = (int)(newSize.Height * aspect);
            }

            return newSize;
        }
    }
}
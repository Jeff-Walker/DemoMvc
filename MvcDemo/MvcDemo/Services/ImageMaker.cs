using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MvcDemo.Services {
    
    public class ImageMaker {
        readonly byte[] _bytes;
        static readonly Size ThumbnailMaximumSize = new Size(500, 700);

        public ImageMaker(byte[] bytes) {
            _bytes = bytes;
        }

        public byte[] MakeThumbnail() {
            using (var ms = new MemoryStream(_bytes)) {
                using (var originalImage = Image.FromStream(ms)) {
                    OriginalSize = originalImage.Size;
                    ThumbnailSize = CalculateNewSize(originalImage);
                    var thumbnail = new Bitmap(originalImage, ThumbnailSize);
                    var outStream = new MemoryStream();
                    thumbnail.Save(outStream, ImageFormat.Png);

                    return outStream.ToArray();
                }
            }
        }
        public Size ThumbnailSize { get; set; }
        public Size OriginalSize { get; set; }

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
            if (aspect > 1.0) {
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
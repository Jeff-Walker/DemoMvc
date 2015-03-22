using System.Drawing;
using System.IO;

namespace MvcDemo.Services {
    public interface IImageMaker {
        Image MakeImage(Stream imageStream, string contentType);
//        Image 
    }
    public class ImageMaker {
         
    }
}
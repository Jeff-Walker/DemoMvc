using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Program {
        static void Main(string[] args) {
            string file = @"C:\Users\Jeff\Documents\blah\bkg-blu.jpg";
            var fileStream = new FileStream(file, FileMode.Open);
            var memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            var array = memoryStream.ToArray();
            var image = Image.FromStream(memoryStream);
            image.Save(@"C:\Users\Jeff\Documents\blah\blah", ImageFormat.Jpeg);
        }
    }
}

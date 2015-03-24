using System.Collections.Generic;

namespace MvcDemo.ViewModels {
    public class ViewUploadViewModel {
        public IEnumerable<UploadedImage> UploadedImages { get; set; } 
    }

    public class UploadedImage {
        public string Id { get; set; }
        public string OriginalFilename { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FullImageUrl { get; set; }
    }
}
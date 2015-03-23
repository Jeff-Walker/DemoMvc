using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace MvcDemo.Services {
    public interface IContentManager {
        IEnumerable<IContentInfo> ListImages();
        string SaveImage(string originalFilename, string contentType, Stream inputStream);
        IContentInfo LoadContentInfo(string id);
        IContent LoadContent(string id);
    }



    public class ContentManager : IContentManager {
        readonly IImageMaker _imageMaker;
        static readonly IDictionary<string, IContentInfo> ContentDatabase = new ConcurrentDictionary<string, IContentInfo>();
        static readonly IDictionary<string, IContent> ContentObjects = new ConcurrentDictionary<string, IContent>();

        public ContentManager(IImageMaker imageMaker) {
            _imageMaker = imageMaker;
        }

        public IEnumerable<IContentInfo> ListImages() {
            return ContentDatabase.Values;
        }

        public string SaveImage(string originalFilename, string contentType, Stream inputStream) {
            var id = NewId();

            var content = new Content() {
                Id = NewId(),
                Bytes = ReadStream(inputStream),
                ContentType = contentType,
            };
            var thumbnailContent = new Content() {
                Id = NewId(),
                Bytes = _imageMaker.MakeThumbnail(content.Bytes),
                ContentType = "image/png",
            };
            
            var contentInfo = new ContentInfo() {
                Id = id,
                OriginalFilename = originalFilename,
                ContentType = contentType,
                ImageId = content.Id,
                ThumbnailId = thumbnailContent.Id,
            };

            ContentDatabase.Add(id, contentInfo);
            ContentObjects.Add(content.Id, content);
            ContentObjects.Add(thumbnailContent.Id, thumbnailContent);

            return id;
        }


        static byte[] ReadStream(Stream inputStream) {
            byte[] bytes;
            using (var memoryStream = new MemoryStream()) {
                inputStream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }

        static string NewId() {
            return Guid.NewGuid().ToString();
        }

        public IContentInfo LoadContentInfo(string id) {
            IContentInfo contentInfo;
            return ContentDatabase.TryGetValue(id, out contentInfo) ? contentInfo : null;
        }

        public IContent LoadContent(string id) {
            IContent content;
            return ContentObjects.TryGetValue(id, out content) ? content : null;
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using MvcDemo.Annotations;

namespace MvcDemo.Services {
    public interface IContentManager {
        IEnumerable<IContentInfo> ListImages();
        void SaveImage(string originalFilename, string contentType, Stream inputStream);
        IContentInfo LoadContentInfo(string id);
        IContent LoadContent(string id);
    }

    /*
     * This is an in-memory content management system. To simulate real life, it has these properties:
     * 1. The metadata seperate from the content.
     * 2. The metadata is cheaper to access than the content.
     * 3. Because of this, you get the metadata and then get the content in a seperate step.
     */

    [UsedImplicitly]
    public class ContentManager : IContentManager {
        static readonly IDictionary<string, IContentInfo> ContentDatabase = new ConcurrentDictionary<string, IContentInfo>();
        static readonly IDictionary<string, IContent> ContentObjects = new ConcurrentDictionary<string, IContent>();


        public IEnumerable<IContentInfo> ListImages() {
            return ContentDatabase.Values;
        }

        public void SaveImage(string originalFilename, string contentType, Stream inputStream) {
            var id = NewId();

            var originalImageBytes = ReadStream(inputStream);
            var imageMaker = new ImageMaker(originalImageBytes);
            var thumbnailBytes = imageMaker.MakeThumbnail();

            var content = new Content() {
                Id = NewId(),
                Bytes = originalImageBytes,
                ContentType = contentType,
                ImageSize = imageMaker.OriginalSize,
            };
            var thumbnailContent = new Content() {
                Id = NewId(),
                Bytes = thumbnailBytes,
                ContentType = "image/png",
                ImageSize = imageMaker.ThumbnailSize,
            };
            
            var contentInfo = new ContentInfo() {
                Id = id,
                OriginalFilename = originalFilename,
                ContentType = contentType,
                ImageId = content.Id,
                ThumbnailId = thumbnailContent.Id,
                CreationDate = DateTime.Now,
                ImageSize = imageMaker.OriginalSize,
            };

            ContentDatabase.Add(id, contentInfo);
            ContentObjects.Add(content.Id, content);
            ContentObjects.Add(thumbnailContent.Id, thumbnailContent);
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
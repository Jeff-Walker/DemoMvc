using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace MvcDemo.Services {
    public interface IContentManager {
        IEnumerable<IContentInfo> ListImages();
        string SaveImage(string filename, string description, string contentType, Stream inputStream);
        IContentInfo LoadContentInfo(string id);
        byte[] LoadContent(string id);
    }

    public interface IContentInfo {
        string Id { get; }
        string OriginalFilename { get; }
        string UserDescription { get; }
        string ContentType { get; }
        string ImageId { get; set; }
        string ThumbnailId { get; set; }
    }

    public enum ImagePartType { FullSize, Thumbnail }

    public class ImagePart {
        public ImagePartType Type { get; set; }
        public string Id { get; set; }
        public byte[] Content { get; set; }
    }

    public class ContentInfo : IEquatable<ContentInfo>, IContentInfo {
        public string Id { get; set; }
        public string OriginalFilename { get; set; }
        public string UserDescription { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public ImagePart Image { get; set; }
        public ImagePart Thumbnail { get; set; }
        public string ImageId { get; set; }
        public string ThumbnailId { get; set; }

        public bool Equals(ContentInfo other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((ContentInfo) obj);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

    }
    public class ContentManager : IContentManager {
        static readonly IDictionary<string, IContentInfo> ContentDatabase = new ConcurrentDictionary<string, IContentInfo>(); 
        static readonly IDictionary<string, byte[]> ContentObjects = new ConcurrentDictionary<string, byte[]>(); 
        public IEnumerable<IContentInfo> ListImages() {
            return ContentDatabase.Values;
        }

        public string SaveImage(string filename, string description, string contentType, Stream inputStream) {
            var id = NewId();

            var imageId = NewId();
            var imageBytes = ReadImage(inputStream);
            
            var thumbnailId = NewId();
            var thumbnailBytes = MakeThumbnail(imageBytes);
            
            var contentInfo = new ContentInfo() {
                Id = id,
                OriginalFilename = filename,
                ContentType = contentType,
                ImageId = imageId,
                ThumbnailId = thumbnailId,
            };
            ContentDatabase.Add(id, contentInfo);
            ContentObjects.Add(imageId, imageBytes);
            ContentObjects.Add(thumbnailId, thumbnailBytes);
            return id;
        }

        byte[] MakeThumbnail(byte[] imageBytes) {
            throw new NotImplementedException();
        }

        static byte[] ReadImage(Stream inputStream) {
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
//            return ContentDatabase[id];
            IContentInfo contentInfo;
            return ContentDatabase.TryGetValue(id, out contentInfo) ? contentInfo : null;
        }

        public byte[] LoadContent(string id) {
//            return ContentObjects[id];
            byte[] bytes;
            return ContentObjects.TryGetValue(id, out bytes) ? bytes : null;
        }
    }
}
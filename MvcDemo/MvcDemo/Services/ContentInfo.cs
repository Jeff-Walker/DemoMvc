using System;

namespace MvcDemo.Services {
   
    public interface IContentInfo {
        string Id { get; }
        string OriginalFilename { get; }
        string UserDescription { get; }
        string ContentType { get; }
        string ImageId { get; set; }
        string ThumbnailId { get; set; }
    }

    public class ContentInfo : IEquatable<ContentInfo>, IContentInfo {
        public string Id { get; set; }
        public string OriginalFilename { get; set; }
        public string UserDescription { get; set; }
        public string ContentType { get; set; }
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

    public interface IContent {
        string Id { get; set; }
        string ContentType { get; set; }
        byte[] Bytes { get; set; }
    }

    public class Content : IContent, IEquatable<Content> {
        public string Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }

        public bool Equals(Content other) {
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
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((Content)obj);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}
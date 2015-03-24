using System.Linq;
using MvcDemo.ViewModels;

namespace MvcDemo.Services {

    public interface IViewModelBuilder<out TViewModel> {
        TViewModel BuildViewModel();

    }

    public class ViewUploadViewModelBuilder : IViewModelBuilder<ViewUploadViewModel> {
        readonly IContentManager _contentManager;

        public ViewUploadViewModelBuilder(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public ViewUploadViewModel BuildViewModel() {
            return new ViewUploadViewModel {
                UploadedImages = _contentManager.ListImages().Select(c =>
                        new UploadedImage {
                            Id = c.Id,
                            OriginalFilename = c.OriginalFilename,
                            ThumbnailUrl = BuildUrl(c.ThumbnailId),
                            FullImageUrl = BuildUrl(c.ImageId)
                        })
            };
        }

        string BuildUrl(string imageId) {
            throw new System.NotImplementedException();
        }
    }
}
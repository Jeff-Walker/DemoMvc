using System.Linq;
using System.Web.Mvc;
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
                            ThumbnailId = c.ThumbnailId,
                            FullImageId = c.ImageId,
                            CreationDate = c.CreationDate,
                            OriginalSize = c.ImageSize,
                        }).OrderByDescending(i=>i.CreationDate)
            };
        }

    }
}
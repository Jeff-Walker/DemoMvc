using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcDemo.Services;
using MvcDemo.ViewModels;

namespace MvcDemo.Controllers {
    public class HomeController : Controller {
        readonly IContentManager _contentManager;
        readonly IViewModelBuilder<ViewUploadViewModel> _viewUploadViewModelBuilder;

        public HomeController(IContentManager contentManager, IViewModelBuilder<ViewUploadViewModel> viewUploadViewModelBuilder) {
            _contentManager = contentManager;
            _viewUploadViewModelBuilder = viewUploadViewModelBuilder;
        }

        public ActionResult Index() {
            return View(_viewUploadViewModelBuilder.BuildViewModel());
        }

      

        [HttpPost]
        public ActionResult Upload(ViewUploadViewModel model) {
            for (var i = 0 ; i < Request.Files.Count ; i++) {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0) {
                    var fileName = Path.GetFileName(file.FileName);
                    _contentManager.SaveImage(fileName, file.ContentType, file.InputStream);
                }
            }
            return View("Index", _viewUploadViewModelBuilder.BuildViewModel());
        }

        [HttpGet]
        public FileResult Image(string id) {
            var content = _contentManager.LoadContent(id);
            return File(content.Bytes, content.ContentType);
        }
    }

    public class ViewUploadViewModelBuilder {
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
                        })
            };
        }

        string BuildUrl(string imageId) {
            throw new System.NotImplementedException();
        }
    }
}
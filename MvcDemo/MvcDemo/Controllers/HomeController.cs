using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcDemo.Services;
using MvcDemo.ViewModels;

namespace MvcDemo.Controllers {
    public class HomeController : Controller {
        readonly IContentManager _contentManager;

        public HomeController(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public ActionResult Index() {
            return View(BuildViewModel());
        }

        ViewUploadViewModel BuildViewModel() {
            _contentManager.ListImages().Select<IContentInfo, UploadedImage>(c => {new UploadedImage {Id=c.Id, OriginalFilename = c.OriginalFilename, ThumbnailUrl = c.}})
            return new ViewUploadViewModel() {
                    
            }
        }

        [HttpPost]
        public ActionResult Upload() {
            for (var i = 0 ; i < Request.Files.Count ; i++) {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0) {
                    var fileName = Path.GetFileName(file.FileName);
                    _contentManager.SaveImage(fileName, file.ContentType, file.InputStream);
                }
            }
            return View("Index", BuildViewModel());
        }
    }
}
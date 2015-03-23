using System.Web.Mvc;
using MvcDemo.Services;

namespace MvcDemo.Controllers {
    public class HomeController : Controller {
        readonly IContentManager _contentManager;

        public HomeController(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public ActionResult Index() {
            return View();
        }
    }
}
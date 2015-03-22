using System.Web.Mvc;

namespace MvcDemo.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
using System.Web.Mvc;

namespace Better4You.UI.Mvc.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToHomeIndex();
            //return View(CurrentUser);
        }
    }
}

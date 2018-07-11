using System.Web.Mvc;
using Better4You.Service;

namespace Better4You.UI.Mvc.Controllers
{
    [Authorize]
    public class LookupController : ControllerBase
    {
        public ILookupService LookupService { get; set; }
        //
        // GET: /LookUp/
        [HttpPost]
        public JsonResult States(string stateName)
        {
            return Json(LookupService.States(stateName), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult Users(string name)
        {
            return Json(LookupService.Users(name), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult Cities(int? stateId, string cityName)
        {
            return Json(LookupService.Cities(stateId,cityName), JsonRequestBehavior.DenyGet);
        }
    }
}

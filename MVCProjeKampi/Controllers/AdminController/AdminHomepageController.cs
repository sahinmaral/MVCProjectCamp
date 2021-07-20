using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminHomepageController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
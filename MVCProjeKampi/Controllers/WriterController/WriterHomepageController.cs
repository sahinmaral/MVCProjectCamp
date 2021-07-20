using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer,User")]
    public class WriterHomepageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
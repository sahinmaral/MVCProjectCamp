using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class ErrorPagesController : Controller
    {
        [AllowAnonymous]
        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            
            return View();
        }
    }
}
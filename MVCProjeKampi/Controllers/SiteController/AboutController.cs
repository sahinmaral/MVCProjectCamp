using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.SiteController
{
    [AllowAnonymous]
    public class AboutController : Controller
    {
        private IAboutService aboutService = new AboutManager(new EfAboutDal());
        public ActionResult Index()
        {
            var about = aboutService.Get(x => x.AboutStatus == true);
            return View(about);
        }
    }
}
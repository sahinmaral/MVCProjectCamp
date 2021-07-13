using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers.SiteController
{

    public class HomepageController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());

        [AllowAnonymous]
        public ActionResult Index()
        {
            var headings = headingService.GetList().OrderByDescending(x => x.HeadingDate).ToList();
            var contents = contentService.GetList().OrderByDescending(x => x.ContentDate).ToList();

            HomepageViewModel viewmodel = new HomepageViewModel();
            viewmodel.Contents = contents;
            viewmodel.Headings = headings;

            return View(viewmodel);
        }

        [AllowAnonymous]
        public PartialViewResult Sidebar()
        {
            var headings = headingService.GetList().OrderByDescending(x => x.HeadingDate).ToList();
            return PartialView(headings);
        }

    }
}
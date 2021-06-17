using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers
{
    public class AboutsController : Controller
    {
        private AboutManager aboutManager = new AboutManager(new EfAboutDal());

        AboutHomepageViewModel viewModel = new AboutHomepageViewModel();

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Index()
        {
            viewModel.Abouts = aboutManager.GetList();
            return View(viewModel);
        }

        public PartialViewResult AboutPartial()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult AddAbout()
        {
            
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult AddAbout(About about)
        {
            aboutManager.Add(about);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult DisableAbout(int id)
        {
            var about = aboutManager.GetById(id);
            about.AboutStatus = false;
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult EnableAbout(int id)
        {
            var count = aboutManager.GetCount(x => x.AboutStatus);
            //if (count>=1)
            //{
            //    Sonrasında alert yapılacak
            //    viewModel.AboutAlertStatus = true;
            //    viewModel.Abouts = aboutManager.GetList();
            //    return RedirectToAction("Index");
            //}
            var about = aboutManager.GetById(id);
            about.AboutStatus = true;
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }
    }
}
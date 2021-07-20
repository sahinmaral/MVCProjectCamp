using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminAboutsController : Controller
    {
        private AboutManager aboutManager = new AboutManager(new EfAboutDal());
        AboutHomepageViewModel viewModel = new AboutHomepageViewModel();

        public ActionResult Index(int p=1)
        {
            viewModel.Abouts = aboutManager.GetList().ToPagedList(p,8);
            viewModel.EnabledAbouts = aboutManager.GetList(x => x.AboutStatus == true).Count;
            viewModel.DisabledAbouts = aboutManager.GetList(x => x.AboutStatus == false).Count;
            
            return View(viewModel);
        }


        public PartialViewResult AboutPartial()
        {
            return PartialView();
        }


        [HttpGet]
        public ActionResult AddAbout()
        {
            
            return View();
        }


        [HttpPost]
        public ActionResult AddAbout(About about)
        {
            aboutManager.Add(about);
            return RedirectToAction("Index");
        }


        public ActionResult DisableAbout(int id)
        {
            var about = aboutManager.GetById(id);
            about.AboutStatus = false;
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }


        public ActionResult EnableAbout(int id)
        {
            foreach (var abouts in aboutManager.GetList())
            {
                abouts.AboutStatus = false;
                aboutManager.Update(abouts);
            }

            var about = aboutManager.GetById(id);
            about.AboutStatus = true;
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult EditAbout(int id)
        {
            var about = aboutManager.GetById(id);
            return View(about);
        }


        [HttpPost]
        public ActionResult EditAbout(About about)
        {
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }
    }
}
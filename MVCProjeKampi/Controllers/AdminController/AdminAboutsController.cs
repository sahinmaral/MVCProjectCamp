using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminAboutsController : Controller
    {
        private AboutManager aboutManager = new AboutManager(new EfAboutDal());

        AboutHomepageViewModel viewModel = new AboutHomepageViewModel();

        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
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

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult AddAbout()
        {
            
            return View();
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult AddAbout(About about)
        {
            aboutManager.Add(about);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult DisableAbout(int id)
        {
            var about = aboutManager.GetById(id);
            about.AboutStatus = false;
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
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

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult EditAbout(int id)
        {
            var about = aboutManager.GetById(id);
            return View(about);
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult EditAbout(About about)
        {
            aboutManager.Update(about);
            return RedirectToAction("Index");
        }
    }
}
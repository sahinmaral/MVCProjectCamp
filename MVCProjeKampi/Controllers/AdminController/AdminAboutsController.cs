using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using BusinessLayer.ValidationRules;
using FluentValidation.Results;
using MVCProjeKampi.Models.ViewModels;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminAboutsController : Controller
    {
        private AboutManager aboutManager = new AboutManager(new EfAboutDal());
        private AboutValidator validator = new AboutValidator();

        public ActionResult Index(int p = 1)
        {

            AboutHomepageViewModel viewModel = new AboutHomepageViewModel();
            viewModel.Abouts = aboutManager.GetList().ToPagedList(p, 8);
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
            ValidationResult results = validator.Validate(about);
            if (results.IsValid)
            {
                aboutManager.Add(about);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var result in results.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }

            }

            return View();
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
            ValidationResult results = validator.Validate(about);
            if (results.IsValid)
            {
                aboutManager.Update(about);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
            }

            return View();
        }
    }
}
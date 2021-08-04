using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using FluentValidation.Results;
using MVCProjeKampi.Models.ViewModels;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminAboutsController : Controller
    {
        private IAboutService _aboutService = new AboutManager(new EfAboutDal());
        private AboutValidator validator = new AboutValidator();

        public ActionResult Index(int p = 1)
        {

            AboutHomepageViewModel viewModel = new AboutHomepageViewModel();
            viewModel.Abouts = _aboutService.GetList().ToPagedList(p, 8);
            viewModel.EnabledAbouts = _aboutService.GetList(x => x.AboutStatus == true).Count;
            viewModel.DisabledAbouts = _aboutService.GetList(x => x.AboutStatus == false).Count;

            return View(viewModel);
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

            var searchedAbout = _aboutService.Get(x => x.AboutHeader == about.AboutHeader);

            if (results.IsValid)
            {

                if (searchedAbout != null)
                {
                    ModelState.AddModelError("AboutHeader", "Böyle bir hakkında başlığı zaten mevcut");
                    return View(about);
                }
                else
                {
                    about.AboutHeaderForFriendlyUrl = UrlSlugHelper.ToUrlSlug(about.AboutHeader);

                    _aboutService.Add(about);
                    return RedirectToAction("Index");
                }

            }
            else
            {

                if (searchedAbout != null)
                {
                    ModelState.AddModelError("AboutHeader", "Böyle bir hakkında başlığı zaten mevcut");
                }

                foreach (var result in results.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }

                return View(about);

            }

        }



        public ActionResult EnableAbout(string aboutHeaderForFriendlyUrl)
        {
            foreach (var abouts in _aboutService.GetList())
            {
                abouts.AboutStatus = false;
                _aboutService.Update(abouts);
            }

            var about = _aboutService.Get(x => x.AboutHeaderForFriendlyUrl == aboutHeaderForFriendlyUrl);
            about.AboutStatus = true;
            _aboutService.Update(about);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult EditAbout(string aboutHeaderForFriendlyUrl)
        {
            var about = _aboutService.Get(x => x.AboutHeaderForFriendlyUrl == aboutHeaderForFriendlyUrl);
            return View(about);
        }


        [HttpPost]
        public ActionResult EditAbout(About about)
        {
            ValidationResult results = validator.Validate(about);
            if (results.IsValid)
            {
                about.AboutHeaderForFriendlyUrl = UrlSlugHelper.ToUrlSlug(about.AboutHeader);

                _aboutService.Update(about);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(about);

        }
    }
}
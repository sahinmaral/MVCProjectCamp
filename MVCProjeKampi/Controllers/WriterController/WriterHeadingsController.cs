using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using PagedList;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer")]
    public class WriterHeadingsController : Controller
    {
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private ICategoryService categoryService = new CategoryManager(new EfCategoryDal());
        private HeadingValidator headingValidator = new HeadingValidator();

        public ActionResult Index(int p = 1)
        {
            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var list = headingService.GetList(x => x.UserId == user.UserId).ToPagedList(p, 8);

            return View(list);
        }


        [HttpGet]
        public ActionResult AddHeading()
        {

            //DropDownList getirir
            List<SelectListItem> categoryValues = (from x in categoryService.GetList()
                                                   select new SelectListItem()
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();

            ViewBag.CategoryValues = categoryValues;

            return View();
        }


        [HttpPost]
        public ActionResult AddHeading(Heading heading)
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            ValidationResult results = headingValidator.Validate(heading);

            if (results.IsValid)
            {
                heading.UserId = user.UserId;
                heading.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                heading.HeadingStatus = true;
                headingService.Add(heading);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var result in results.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }
            }

            List<SelectListItem> categoryValues = (from x in categoryService.GetList()
                                                   select new SelectListItem()
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();

            ViewBag.CategoryValues = categoryValues;
            return View();

        }


        [HttpGet]
        public ActionResult EditHeading(string headingNameForFriendlyUrl)
        {
            var headingValue = headingService.Get(x=>x.HeadingNameForFriendlyUrl == headingNameForFriendlyUrl);
            return View(headingValue);
        }


        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {

            ValidationResult results = headingValidator.Validate(heading);
            if (results.IsValid)
            {
                headingService.Update(heading);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                var category = categoryService.GetById(heading.CategoryId);
                heading.Category = category;
            }

            return View(heading);
        }


        public ActionResult DeleteHeading(string headingNameForFriendlyUrl)
        {
            var headingValue = headingService.Get(x=>x.HeadingNameForFriendlyUrl==headingNameForFriendlyUrl);
            headingValue.HeadingStatus = false;
            headingService.Delete(headingValue);
            return RedirectToAction("Index");
        }
    }
}

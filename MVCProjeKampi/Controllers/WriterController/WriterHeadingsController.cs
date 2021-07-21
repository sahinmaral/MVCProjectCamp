using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using PagedList;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer,User")]
    public class WriterHeadingsController : Controller
    {
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private ICategoryService categoryService = new CategoryManager(new EfCategoryDal());
        private HeadingValidator headingValidator = new HeadingValidator();

        public ActionResult Index(int p = 1)
        {
            var username = Session["Username"];

            var writer = writerService.Get(x => x.User.UserUsername == username.ToString());

            var list = headingService.GetList(x => x.WriterId == writer.WriterId).ToPagedList(p, 8);

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
            var user = writerService.Get(x => x.User.UserUsername == username.ToString());

            ValidationResult results = headingValidator.Validate(heading);

            if (results.IsValid)
            {
                heading.WriterId = user.WriterId;
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
        public ActionResult EditHeading(int id)
        {
            var headingValue = headingService.GetById(id);
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
            }

            return View(heading);
        }


        public ActionResult DeleteHeading(int id)
        {
            var headingValue = headingService.GetById(id);
            headingValue.HeadingStatus = false;
            headingService.Delete(headingValue);
            return RedirectToAction("Index");
        }
    }
}

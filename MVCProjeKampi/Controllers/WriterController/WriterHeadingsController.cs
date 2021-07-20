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
        private IWriterService _writerService = new WriterManager(new EfWriterDal(), new EfUserDal());
        private IHeadingService _headingService = new HeadingManager(new EfHeadingDal());
        private ICategoryService _categoryService = new CategoryManager(new EfCategoryDal());


        public ActionResult Index(int p=1)
        {
            var username = Session["Username"];

            var writer = _writerService.Get(x => x.User.UserUsername == username.ToString());

            var list = _headingService.GetList(x => x.WriterId == writer.WriterId).ToPagedList(p,8);
            
            return View(list);
        }


        [HttpGet]
        public ActionResult AddHeading()
        {

            //DropDownList getirir
            List<SelectListItem> categoryValues = (from x in _categoryService.GetList()
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
            var user = _writerService.Get(x => x.User.UserUsername == username.ToString());

            HeadingValidator headingValidator = new HeadingValidator();
            ValidationResult results = headingValidator.Validate(heading);
            if (results.IsValid)
            {
                heading.WriterId = user.WriterId;
                heading.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                heading.HeadingStatus = true;
                _headingService.Add(heading);
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


        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> categoryValues = (from x in _categoryService.GetList()
                                                   select new SelectListItem()
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();


            List<SelectListItem> writerValues = (from x in _writerService.GetWriterDetails()
                                                 select new SelectListItem()
                                                 {
                                                     Text = $"{x.User.UserFirstName} {x.User.UserLastName}",
                                                     Value = x.WriterId.ToString()
                                                 }).ToList();



            var headingValue = _headingService.GetById(id);

            if (headingValue == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CategoryValues = categoryValues;
            ViewBag.WriterValues = writerValues;
            return View(headingValue);
        }


        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            _headingService.Update(heading);
            return RedirectToAction("Index");
        }


        public ActionResult DeleteHeading(int id)
        {
            var headingValue = _headingService.GetById(id);
            headingValue.HeadingStatus = false;
            _headingService.Delete(headingValue);
            return RedirectToAction("Index");
        }
    }
}

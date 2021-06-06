using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntitiesLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers
{
    public class HeadingsController : Controller
    {
        private IBaseService<Heading> headingManager = new HeadingManager(new EfHeadingDal());
        private IBaseService<Category> categoryManager = new CategoryManager(new EfCategoryDal());
        private IBaseService<Writer> writerManager = new WriterManager(new EfWriterDal());
        public ActionResult Index()
        {
            var headingValues = headingManager.GetList();
            return View(headingValues);
        }
        [HttpGet]
        public ActionResult AddHeading()
        {
            //DropDownList getirir
            List<SelectListItem> categoryValues = (from x in categoryManager.GetList()
                select new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value=x.CategoryId.ToString()
                }).ToList();


            List<SelectListItem> writerValues = (from x in writerManager.GetList()
                select new SelectListItem()
                {
                    Text = $"{x.WriterName} {x.WriterSurname}",
                    Value = x.WriterId.ToString()
                }).ToList();

            ViewBag.CategoryValues = categoryValues;
            ViewBag.WriterValues = writerValues;
            return View();
        }
        [HttpPost]
        public ActionResult AddHeading(Heading heading)
        {
            HeadingValidator headingValidator = new HeadingValidator();
            ValidationResult results = headingValidator.Validate(heading);
            if (results.IsValid)
            {
                heading.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                headingManager.Add(heading);
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
            List<SelectListItem> categoryValues = (from x in categoryManager.GetList()
                select new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();


            List<SelectListItem> writerValues = (from x in writerManager.GetList()
                select new SelectListItem()
                {
                    Text = $"{x.WriterName} {x.WriterSurname}",
                    Value = x.WriterId.ToString()
                }).ToList();

            ViewBag.CategoryValues = categoryValues;
            ViewBag.WriterValues = writerValues;

            var headingValue = headingManager.GetById(id);
            return View(headingValue);
        }

        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            headingManager.Update(heading);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteHeading(int id)
        {
            var headingValue = headingManager.GetById(id);
            headingValue.HeadingStatus = false;
            headingManager.Delete(headingValue);
            return RedirectToAction("Index");
        }
    }
}
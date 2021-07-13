using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminHeadingsController : Controller
    {
        private IHeadingService _headingManager = new HeadingManager(new EfHeadingDal());
        private ICategoryService _categoryManager = new CategoryManager(new EfCategoryDal());
        private IWriterService _writerManager = new WriterManager(new EfWriterDal(),new EfUserDal());


        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Index()
        {
            var headingValues = _headingManager.GetList();

            foreach (var items in _writerManager.GetWriterDetails())
            {
                foreach (var headingValue in headingValues)
                {
                    if (headingValue.WriterId == items.WriterId)
                    {
                        headingValue.Writer.User = items.User;
                    }
                }
            }

            return View(headingValues);
        }


        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult AddHeading()
        {
            //DropDownList getirir
            List<SelectListItem> categoryValues = (from x in _categoryManager.GetList()
                select new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value=x.CategoryId.ToString()
                }).ToList();


            List<SelectListItem> writerValues = (from x in _writerManager.GetList()
                select new SelectListItem()
                {
                    Text = $"{x.User.UserFirstName} {x.User.UserLastName}",
                    Value = x.WriterId.ToString()
                }).ToList();

            ViewBag.CategoryValues = categoryValues;
            ViewBag.WriterValues = writerValues;
            return View();
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult AddHeading(Heading heading)
        {
            HeadingValidator headingValidator = new HeadingValidator();
            ValidationResult results = headingValidator.Validate(heading);
            if (results.IsValid)
            {
                heading.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                _headingManager.Add(heading);
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

        
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> categoryValues = (from x in _categoryManager.GetList()
                select new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();


            List<SelectListItem> writerValues = (from x in _writerManager.GetWriterDetails()
                select new SelectListItem()
                {
                    Text = $"{x.User.UserFirstName} {x.User.UserLastName}",
                    Value = x.WriterId.ToString()
                }).ToList();

            

            var headingValue = _headingManager.GetById(id);

            if (headingValue==null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CategoryValues = categoryValues;
            ViewBag.WriterValues = writerValues;
            return View(headingValue);
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            _headingManager.Update(heading);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult EnableHeading(int id)
        {
            var heading = _headingManager.GetById(id);
            heading.HeadingStatus = true;
            _headingManager.Update(heading);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult DeleteHeading(int id)
        {
            var headingValue = _headingManager.GetById(id);
            headingValue.HeadingStatus = false;
            _headingManager.Delete(headingValue);
            return RedirectToAction("Index");
        }
    }
}
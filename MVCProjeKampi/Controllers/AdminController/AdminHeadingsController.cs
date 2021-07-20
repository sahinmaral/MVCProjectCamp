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
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{

    [Authorize(Roles = "Administrator")]
    public class AdminHeadingsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private ICategoryService categoryService = new CategoryManager(new EfCategoryDal());
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());

        public ActionResult Index(int p = 1)
        {
            var headingValues = headingService.GetList().OrderByDescending(x => x.HeadingDate).ToPagedList(p, 8);

            foreach (var items in writerService.GetWriterDetails())
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


        public ActionResult MyHeadings(int p = 1)
        {
            var username = Session["Username"];
            var writer = writerService.Get(x => x.User.UserUsername == username.ToString());

            var headingValues = headingService.GetList(x=>x.WriterId == writer.WriterId).OrderByDescending(x => x.HeadingDate).ToPagedList(p, 8);

            foreach (var items in writerService.GetWriterDetails())
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

            HeadingValidator headingValidator = new HeadingValidator();
            ValidationResult results = headingValidator.Validate(heading);
            if (results.IsValid)
            {
                heading.WriterId = user.WriterId;
                heading.HeadingDate = DateTime.Now;
                headingService.Add(heading);
                return RedirectToAction("MyHeadings");
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
            var heading = headingService.GetById(id);

            return View(heading);
        }



        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            headingService.Update(heading);
            return RedirectToAction("Index");
        }


        public ActionResult EnableHeading(int id)
        {
            var heading = headingService.GetById(id);
            heading.HeadingStatus = true;
            headingService.Update(heading);
            return RedirectToAction("Index");

        }



        public ActionResult DeleteHeading(int id)
        {
            var headingValue = headingService.GetById(id);
            headingValue.HeadingStatus = false;
            headingService.Delete(headingValue);
            return RedirectToAction("Index");
        }


        public ActionResult HeadingReport()
        {
            var headings = headingService.GetList();

            var categories = categoryService.GetList();
            var writers = writerService.GetWriterDetails();

            foreach (var heading in headings)
            {
                foreach (var category in categories)
                {
                    if (heading.CategoryId == category.CategoryId)
                    {
                        heading.Category = category;
                    }
                }

                foreach (var writer in writers)
                {
                    if (heading.WriterId == writer.WriterId)
                    {
                        heading.Writer = writer;
                    }
                }
            }

            return View(headings);
        }


        public ActionResult HeadingsByCategory(int id, int p = 1)
        {
            var headings = headingService.GetList(x => x.CategoryId == id).
                OrderByDescending(x => x.HeadingDate).ToPagedList(p, 8);

            foreach (var items in writerService.GetWriterDetails())
            {
                foreach (var headingValue in headings)
                {
                    if (headingValue.WriterId == items.WriterId)
                    {
                        headingValue.Writer.User = items.User;
                    }
                }
            }

            return View(headings);
        }


    }
}
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

    [Authorize(Roles = "Administrator,User")]
    [Authorize(Roles = "Moderator,User")]
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


            List<SelectListItem> writerValues = (from x in writerService.GetWriterDetails()
                                                 select new SelectListItem()
                                                 {
                                                     Text = $"{x.User.UserFirstName} {x.User.UserLastName}",
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
            return View();

        }



        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> categoryValues = (from x in categoryService.GetList()
                                                   select new SelectListItem()
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();


            List<SelectListItem> writerValues = (from x in writerService.GetWriterDetails()
                                                 select new SelectListItem()
                                                 {
                                                     Text = $"{x.User.UserFirstName} {x.User.UserLastName}",
                                                     Value = x.WriterId.ToString()
                                                 }).ToList();



            var headingValue = headingService.GetById(id);

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
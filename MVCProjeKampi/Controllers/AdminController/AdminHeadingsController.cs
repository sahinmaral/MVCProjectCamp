using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using PagedList;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers.AdminController
{

    [Authorize(Roles = "Administrator")]
    public class AdminHeadingsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private ICategoryService categoryService = new CategoryManager(new EfCategoryDal());
        private HeadingValidator headingValidator = new HeadingValidator();
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        public ActionResult Index(int p = 1)
        {
            var headingValues = headingService.GetList().OrderByDescending(x => x.HeadingDate).ToPagedList(p, 8);

            foreach (var items in userService.GetList())
            {
                foreach (var headingValue in headingValues)
                {
                    headingValue.User.UserId = items.UserId;
                }
            }

            return View(headingValues);
        }


        public ActionResult MyHeadings(int p = 1)
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var headingValues = headingService.GetList(x=>x.UserId == user.UserId).OrderByDescending(x => x.HeadingDate).ToPagedList(p, 8);

            foreach (var items in userService.GetList())
            {
                foreach (var headingValue in headingValues)
                {
                    headingValue.User = items;
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

            ViewBag.Categories = categoryValues;

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

            List<SelectListItem> categoryValues = (from x in categoryService.GetList()
                select new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();

            ViewBag.Categories = categoryValues;

            return View();

        }


        [HttpGet]
        public ActionResult EditHeading(string headingNameForFriendlyUrl)
        {
            var heading = headingService.Get(x=>x.HeadingNameForFriendlyUrl==headingNameForFriendlyUrl);

            return View(heading);
        }



        [HttpPost]
        public ActionResult EditHeading(Heading heading)
        {
            HeadingValidator headingValidator = new HeadingValidator();
            ValidationResult results = headingValidator.Validate(heading);
            if (results.IsValid)
            {
                heading.HeadingNameForFriendlyUrl = UrlSlugHelper.ToUrlSlug(heading.HeadingName);
                headingService.Update(heading);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var result in results.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }

                var category = categoryService.GetById(heading.CategoryId);
                heading.Category = category;
            }

            

            return View(heading);
        }


        public ActionResult EnableHeading(string headingNameForFriendlyUrl)
        {
            var heading = headingService.Get(x=>x.HeadingNameForFriendlyUrl==headingNameForFriendlyUrl);
            heading.HeadingStatus = true;
            headingService.Update(heading);
            return RedirectToAction("Index");

        }



        public ActionResult DeleteHeading(string headingNameForFriendlyUrl)
        {
            var headingValue = headingService.Get(x=>x.HeadingNameForFriendlyUrl==headingNameForFriendlyUrl);
            headingValue.HeadingStatus = false;
            headingService.Delete(headingValue);
            return RedirectToAction("Index");
        }


        public ActionResult HeadingReport()
        {
            var headings = headingService.GetList();

            var categories = categoryService.GetList();
            var users = userService.GetList();

            foreach (var heading in headings)
            {
                foreach (var category in categories)
                {
                    if (heading.CategoryId == category.CategoryId)
                    {
                        heading.Category = category;
                    }
                }

                foreach (var user in users)
                {
                    if (heading.UserId == user.UserId)
                    {
                        heading.User = user;
                    }
                }
            }

            return View(headings);
        }


        public ActionResult HeadingsByCategory(string categoryNameForFriendlyUrl, int p = 1)
        {
            var headings = headingService.GetList(x => x.Category.CategoryNameForFriendlyUrl == categoryNameForFriendlyUrl).
                OrderByDescending(x => x.HeadingDate).ToPagedList(p, 8);

            foreach (var items in userService.GetList())
            {
                foreach (var headingValue in headings)
                {
                    headingValue.User.UserId = items.UserId;
                }
            }

            return View(headings);
        }

        public ActionResult HeadingCalendar()
        {
            return View();
        }

        public ActionResult GetHeadingCalendarDatas()
        {
            List<HeadingCalendarViewModel> viewmodel = new List<HeadingCalendarViewModel>();

            var headings = headingService.GetList();

            foreach (var heading in headings)
            {
                HeadingCalendarViewModel item = new HeadingCalendarViewModel()
                {
                    backgroundColor = "gray",
                    start = heading.HeadingDate,
                    end = heading.HeadingDate,
                    title = heading.HeadingName,
                    url = "/baslik/" + heading.HeadingNameForFriendlyUrl
                };
                viewmodel.Add(item);
            }

            return Json(viewmodel.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}
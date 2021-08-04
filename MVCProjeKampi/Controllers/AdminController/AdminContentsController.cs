using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation.Results;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminContentsController : Controller
    {
        private IContentService contentService = new ContentManager(new EfContentDal());

        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        private ContentValidator validator = new ContentValidator();


        public ActionResult MyContentsByHeading(int p = 1)
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var contentValues = contentService.GetList(x => x.UserId == user.UserId && x.Heading.HeadingStatus == true).ToPagedList(p, 9);

            return View(contentValues);
        }

        [HttpGet]
        public ActionResult EditContent(int id)
        {
            var headingValue = contentService.GetById(id);
            return View(headingValue);
        }


        [HttpPost]
        public ActionResult EditContent(Content content)
        {
            ValidationResult results = validator.Validate(content);

            if (results.IsValid)
            {
                var heading = headingService.GetById(content.HeadingId);

                contentService.Update(content);
                return RedirectToAction("HeadingByHeadingNameForFriendlyUrl","Headings" , new { headingNameForFriendlyUrl = content.Heading.HeadingNameForFriendlyUrl});
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(content);
        }

        [HttpGet]
        public ActionResult EditMyContent(int id)
        {
            var headingValue = contentService.GetById(id);
            return View(headingValue);
        }


        [HttpPost]
        public ActionResult EditMyContent(Content content)
        {
            ValidationResult results = validator.Validate(content);

            if (results.IsValid)
            {
                var heading = headingService.GetById(content.HeadingId);

                contentService.Update(content);
                return RedirectToAction("HeadingByHeadingNameForFriendlyUrl", "Headings", new { headingNameForFriendlyUrl = heading.HeadingNameForFriendlyUrl });
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(content);
        }

    }
}
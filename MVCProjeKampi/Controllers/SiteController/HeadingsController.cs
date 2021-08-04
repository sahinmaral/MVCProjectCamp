using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using MVCProjeKampi.Models.ViewModels;

using PagedList;

using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class HeadingsController : Controller
    {
        private IHeadingService _headingService = new HeadingManager(new EfHeadingDal());
        private IContentService _contentService = new ContentManager(new EfContentDal());

        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));


        private ContentValidator validator = new ContentValidator();


        [AllowAnonymous]
        public ActionResult HeadingByWriterUsername(string username, int pageNumber = 1)
        {
            var foundUser = _userService.Get(x => x.UserUsername == username);

            var headings = _headingService.GetList().
                OrderByDescending(x => x.HeadingDate).Where(x => x.UserId == foundUser.UserId).ToPagedList(pageNumber, 8);

            HeadingsByWriterViewModel headingsByWriterViewModel = new HeadingsByWriterViewModel()
            {
                Headings = headings,
                User = foundUser
            };

            return View(headingsByWriterViewModel);

        }



        [AllowAnonymous]
        public ActionResult HeadingByHeadingNameForFriendlyUrl(string headingNameForFriendlyUrl, int pageNumber = 1)
        {

            var heading = _headingService.Get(x => x.HeadingNameForFriendlyUrl == headingNameForFriendlyUrl);

            if (heading == null)
            {
                return RedirectToAction("NoHeading", "Headings" , new {searchedHeadingNameForFriendlyUrl  = headingNameForFriendlyUrl});
            }
            
            else if (heading.HeadingStatus == false)
            {
                return RedirectToAction("DeletedHeading", "Headings", new { searchedHeadingNameForFriendlyUrl = headingNameForFriendlyUrl });
            }

            else
            {
                var contents = _contentService.GetList(x => x.HeadingId == heading.HeadingId).ToPagedList(pageNumber, 8);
                var users = _userService.GetList();

                ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
                viewModel.ContentList = contents;
                viewModel.Heading = heading;

                foreach (var content in contents)
                {
                    foreach (var user in users)
                    {
                        if (content.UserId == user.UserId)
                        {
                            content.User = user;
                        }
                    }
                }

                ViewBag.ContentsByHeadingViewModel = viewModel;

                return View();
            }

            
        }

        [AllowAnonymous]
        public ActionResult HeadingByHeadingName(string headingName)
        {
            var name = UrlSlugHelper.ToUrlSlug(headingName);
            return RedirectToAction("HeadingByHeadingNameForFriendlyUrl", "Headings", new { headingNameForFriendlyUrl = name });
        }



        [AllowAnonymous]
        public JsonResult GetHeadings()
        {
            return new JsonResult
            {
                Data = _headingService.GetList().Select(x => x.HeadingName).ToArray(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult WriteContentOnHeading(Content content)
        {

            var username = Session["Username"];

            var user = _userService.Get(x => x.UserUsername == username.ToString());

            var writer = _userService.Get(x => x.UserId == user.UserId);

            var heading = _headingService.Get(x => x.HeadingId == content.Heading.HeadingId);

            content.Heading = heading;
            content.UserId = writer.UserId;
            content.HeadingId = heading.HeadingId;

            Content newContent = new Content();
            newContent.ContentDate = DateTime.Now;
            newContent.UserId = content.UserId;
            newContent.HeadingId = content.HeadingId;
            newContent.ContentText = content.ContentText;

            ValidationResult results = validator.Validate(newContent);

            if (results.IsValid)
            {
                _contentService.Add(newContent);
                return RedirectToAction("HeadingByHeadingNameForFriendlyUrl", "Headings", new { headingNameForFriendlyUrl = content.Heading.HeadingNameForFriendlyUrl });
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                var refreshedHeading = _headingService.GetById(content.HeadingId);
                var refreshedContents = _contentService.GetList(x => x.HeadingId == content.HeadingId).ToPagedList(1, 8);
                var refreshedWriters = _userService.GetList();

                ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
                viewModel.ContentList = refreshedContents;
                viewModel.Heading = heading;

                foreach (var refreshedContent in refreshedContents)
                {
                    foreach (var refreshedWriter in refreshedWriters)
                    {
                        if (refreshedContent.UserId == refreshedWriter.UserId)
                        {
                            refreshedContent.User = refreshedWriter;
                        }
                    }
                }

                ViewBag.ContentsByHeadingViewModel = viewModel;

            }

            return View("HeadingByHeadingNameForFriendlyUrl");
        }

        [AllowAnonymous]
        public ActionResult NoHeading(string searchedHeadingNameForFriendlyUrl)
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DeletedHeading(string searchedHeadingNameForFriendlyUrl)
        {
            return View();
        }

    }
}
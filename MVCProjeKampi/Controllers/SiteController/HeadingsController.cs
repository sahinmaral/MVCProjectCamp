using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using MVCProjeKampi.Models.ViewModels;

using PagedList;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class HeadingsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));


        private ContentValidator validator = new ContentValidator();


        [AllowAnonymous]
        public ActionResult HeadingByWriterUsername(string username, int pageNumber = 1)
        {
            var user = userService.Get(x=>x.UserUsername == username);

            var headings = headingService.GetList().
                OrderByDescending(x => x.HeadingDate).Where(x => x.UserId == user.UserId).ToPagedList(pageNumber, 8);

            HeadingsByWriterViewModel headingsByWriterViewModel = new HeadingsByWriterViewModel()
            {
                Headings = headings,
                User = user
            };

            return View(headingsByWriterViewModel);

        }

        /// <summary>
        ///
        /// Hata1 : 
        /// Jeff Bezos'un astronot olması -> Jeff Bezos
        /// </summary>
        /// <param name="headingNameForFriendlyUrl"></param>
        /// <returns></returns>

        [AllowAnonymous]
        public ActionResult HeadingByHeadingNameForFriendlyUrl(string headingNameForFriendlyUrl,int pageNumber = 1)
        {

            var heading = headingService.Get(x => x.HeadingNameForFriendlyUrl == headingNameForFriendlyUrl);

            var contents = contentService.GetList(x => x.HeadingId == heading.HeadingId).ToPagedList(pageNumber, 8);
            var writers = userService.GetList();

            ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
            viewModel.ContentList = contents;
            viewModel.Heading = heading;

            foreach (var content in contents)
            {
                foreach (var user in writers)
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

        [AllowAnonymous]
        public ActionResult HeadingByHeadingName(string headingName)
        {
            var name = UrlSlugHelper.ToUrlSlug(headingName);
            return RedirectToAction("HeadingByHeadingNameForFriendlyUrl", "Headings" , new {headingNameForFriendlyUrl = name});
        }

        [AllowAnonymous]
        public ActionResult SearchHeadings(string searched)
        {
            //Search bar araştırılacak
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetHeadings()
        {
            var headings = headingService.GetList();

            List<string> headingNames = new List<string>(); 

            for (int i = 0; i < headings.Count; i++)
            {
                headingNames.Add(headings[i].HeadingName); 
            }

            return new JsonResult { Data = headingNames.ToArray(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult WriteContentOnHeading(Content content)
        {

            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var writer = userService.Get(x => x.UserId == user.UserId);

            var heading = headingService.Get(x => x.HeadingId == content.Heading.HeadingId);

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
                contentService.Add(newContent);
                return RedirectToAction("HeadingByHeadingNameForFriendlyUrl", "Headings", new { headingByHeadingNameForFriendlyUrl = content.Heading.HeadingNameForFriendlyUrl });
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                var refreshedHeading = headingService.GetById(content.HeadingId);
                var refreshedContents = contentService.GetList(x => x.HeadingId == content.HeadingId).ToPagedList(1, 8);
                var refreshedWriters = userService.GetList();

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

    }
}
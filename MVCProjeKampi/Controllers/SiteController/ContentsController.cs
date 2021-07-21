using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using MVCProjeKampi.Models.ViewModels;

using System;
using System.Web.Mvc;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class ContentsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());
        private ContentValidator validator = new ContentValidator();

        [HttpPost]
        [Authorize(Roles = "Writer,User")]
        public ActionResult WriteContentOnHeading(ContentsByHeadingViewModel viewmodel)
        {

            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var writer = writerService.Get(x => x.UserId == user.UserId);

            var heading = headingService.Get(x => x.HeadingName == viewmodel.Heading.HeadingName);
            viewmodel.GoingToAddContent.Heading = heading;
            viewmodel.GoingToAddContent.WriterId = writer.WriterId;
            viewmodel.GoingToAddContent.HeadingId = heading.HeadingId;


            Content content = new Content();
            content.ContentDate = DateTime.Now;
            content.WriterId = viewmodel.GoingToAddContent.WriterId;
            content.HeadingId = viewmodel.GoingToAddContent.HeadingId;
            content.ContentText = viewmodel.GoingToAddContent.ContentText;

            ValidationResult results = validator.Validate(content);

            if (results.IsValid)
            {
                contentService.Add(content);
                return RedirectToAction("HeadingByHeadingId", "Headings", new { id = viewmodel.GoingToAddContent.Heading.HeadingId });
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
            }

            return RedirectToAction("HeadingByHeadingId", "Headings", new { id = viewmodel.GoingToAddContent.Heading.HeadingId });



        }
    }
}
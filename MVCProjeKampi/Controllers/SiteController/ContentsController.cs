using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System;
using System.Web.Mvc;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class ContentsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());

        [HttpPost]
        [Authorize(Roles = "Writer,User")]
        public ActionResult WriteContentOnHeading(ContentsByHeadingViewModel viewmodel)
        {

            var username = Session["Username"];

            if (username.Equals(null))
            {
                //Burası düzeltilecek
                return RedirectToAction("Login", "Logins");
            }


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
            
            contentService.Add(content);

            return RedirectToAction("HeadingByHeadingId", "Headings" , new {id = viewmodel.GoingToAddContent.Heading.HeadingId});


        }
    }
}
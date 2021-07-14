using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminContentsController : Controller
    {
        private IContentService contentManager = new ContentManager(new EfContentDal());
        private IUserService userService = new UserManager(new EfUserDal(),new EfSkillDal(),new RoleManager(new EfRoleDal(),new EfUserDal(),new EfUserRoleDal()));
        private IWriterService writerService = new WriterManager(new EfWriterDal(),new EfUserDal());

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        public ActionResult ContentByHeading(int id)
        {
            string headingName = String.Empty;
            var contentValues = contentManager.GetList(x => x.HeadingId == id);
            
            foreach (var content in contentValues)
            {
                var writer = writerService.GetById(content.WriterId);
                content.Writer.User = userService.GetById(writer.UserId);
            }

            var headingContent = contentValues.FirstOrDefault();
            if (headingContent != null)
            {
                headingName = headingContent.Heading.HeadingName;
            }

            ContentsByHeadingViewModel model1 = new ContentsByHeadingViewModel()
            {
                ContentList = contentValues,
                HeadingName = headingName
            };

            return View(model1);
        }


    }
}
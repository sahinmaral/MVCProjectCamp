using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.CodeAnalysis;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers
{
    public class ContentsController : Controller
    {
        private IContentService contentManager = new ContentManager(new EfContentDal());
        private IUserService userService = new UserManager(new EfUserDal(),new EfSkillDal());
        private IWriterService writerService = new WriterManager(new EfWriterDal(),new EfUserDal());

        [Authorize(Roles = "Admin,User")]
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

            ContentByHeadingViewModel model1 = new ContentByHeadingViewModel()
            {
                ContentList = contentValues,
                HeadingName = headingName
            };

            return View(model1);
        }


    }
}
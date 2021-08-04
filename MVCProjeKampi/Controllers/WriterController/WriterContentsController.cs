using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using PagedList;

using System.Web.Mvc;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer")]
    public class WriterContentsController : Controller
    {
        private IContentService _contentService = new ContentManager(new EfContentDal());

        private IHeadingService _headingService = new HeadingManager(new EfHeadingDal());

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        private ContentValidator validator = new ContentValidator();


        public ActionResult MyContentsByHeading(int p=1)
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var contentValues = _contentService.GetList(x=>x.UserId==user.UserId && x.Heading.HeadingStatus == true).ToPagedList(p,9);

            return View(contentValues);
        }

        [HttpGet]
        public ActionResult EditContent(int id)
        {
            //Eğer yazar başkasının başlığına erişirse ?

            var headingValue = _contentService.GetById(id);
            return View(headingValue);
        }


        [HttpPost]
        public ActionResult EditContent(Content content)
        {
            ValidationResult results = validator.Validate(content);

            if (results.IsValid)
            {
                var heading = _headingService.GetById(content.HeadingId);

                _contentService.Update(content);
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
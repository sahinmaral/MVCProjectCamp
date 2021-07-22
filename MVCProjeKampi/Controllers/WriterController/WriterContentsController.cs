using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using PagedList;

using System.Web.Mvc;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer,User")]
    public class WriterContentsController : Controller
    {
        private IContentService contentManager = new ContentManager(new EfContentDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(), new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        //Başlığa göre arama yapılabilir

        public ActionResult ContentByHeading(int p=1)
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var contentValues = contentManager.GetList(x=>x.Writer.UserId==user.UserId).ToPagedList(p,9);

            return View(contentValues);
        }

        //[HttpGet]
        //public ActionResult EditContent(int id)
        //{
        //    var headingValue = headingService.GetById(id);
        //    return View(headingValue);
        //}


        //[HttpPost]
        //public ActionResult EditContent(Heading heading)
        //{

        //    ValidationResult results = headingValidator.Validate(heading);
        //    if (results.IsValid)
        //    {
        //        headingService.Update(heading);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        foreach (var item in results.Errors)
        //        {
        //            ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
        //        }
        //    }

        //    return View(heading);
        //}
    }
}
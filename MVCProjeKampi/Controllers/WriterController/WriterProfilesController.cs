using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer")]
    public class WriterProfilesController : Controller
    {

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        private UserValidator validator = new UserValidator();

        [HttpGet]
        public ActionResult EditProfile()
        {
            var username = Session["Username"].ToString();

            var user = userService.Get(x => x.UserUsername == username.ToString());

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            var username = Session["Username"].ToString();

            ValidationResult results = validator.Validate(user);


            if (results.IsValid)
            {
                var foundUser = userService.Get(x => x.UserUsername == username);

                foundUser.UserFirstName = user.UserFirstName;
                foundUser.UserLastName = user.UserLastName;
                foundUser.UserImage = user.UserImage;
                foundUser.UserAbout = user.UserAbout;
                foundUser.UserTitle = user.UserTitle;

                Session["Username"] = foundUser.UserUsername;
                Session["UserImage"] = foundUser.UserImage;
                Session["Fullname"] = foundUser.UserFirstName + " " + foundUser.UserLastName;

                userService.Update(foundUser);
                return RedirectToAction("", "Homepage");
            }

            foreach (var item in results.Errors)
            {
                ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
            }

            return View();
        }
    }
}
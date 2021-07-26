using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using PagedList;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{

    [Authorize(Roles = "Administrator")]
    public class AdminWritersController : Controller
    {
        private UserValidator writerValidator = new UserValidator();


        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));


        public ActionResult Index(int p=1)
        {
            var writerValues = userService.GetList().ToPagedList(p,8);
            
            return View(writerValues);
        }

        [HttpGet]
        public ActionResult EditWriter(string username)
        {
            var user = userService.Get(x=>x.UserUsername == username);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditWriter(User user)
        {
            var foundUser = userService.GetById(user.UserId);

            ValidationResult result = writerValidator.Validate(user);
            if (result.IsValid)
            {
                foundUser.UserUsername = user.UserUsername;
                foundUser.UserLastName = user.UserLastName;
                foundUser.UserFirstName = user.UserFirstName;
                foundUser.UserAbout = user.UserAbout;
                foundUser.UserTitle = user.UserTitle;
                foundUser.UserImage = user.UserImage;

                userService.Update(foundUser);

                Session["Username"] = user.UserUsername;
                Session["UserImage"] = user.UserImage;
                Session["Fullname"] = user.UserFirstName + " " + user.UserLastName;

                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
            }

            return View(user);
        }

    }
}
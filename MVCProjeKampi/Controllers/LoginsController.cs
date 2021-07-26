using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.DTOs;

using FluentValidation.Results;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class LoginsController : Controller
    {
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
            new EfUserDal(), new EfUserRoleDal()));

        private UserForRegisterDtoValidator validator = new UserForRegisterDtoValidator();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            userService.Logout();
            return RedirectToAction("Index", "Homepage");
        }

        /// <summary>
        /// Böyle bir hesap var mı , kullanıcı adı ve şifre uyuşuyor mu gibi mesajları
        /// toastr üzerinden kullanıcıya yollayabiliriz 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserForLoginDto user)
        {
            if (userService.LoginAdmin(user) || userService.LoginWriter(user))
            {
                return RedirectToAction("Index", "Homepage");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Registration(UserForRegisterDto entity)
        {
            ValidationResult result = validator.Validate(entity);
            if (result.IsValid)
            {
                userService.Register(entity);
                return RedirectToAction("Login", "Logins");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(entity);
        }


    }
}
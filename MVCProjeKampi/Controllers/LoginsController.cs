using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.DTOs;

using FluentValidation.Results;

using Microsoft.Ajax.Utilities;

using System;
using System.IO;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class LoginsController : Controller
    {
        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
            new EfUserDal(), new EfUserRoleDal()));

        private UserForRegisterDtoValidator registerValidator = new UserForRegisterDtoValidator();
        private UserForLoginDtoValidator loginValidator = new UserForLoginDtoValidator();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new UserForLoginDto());
        }

        [HttpGet]
        public ActionResult Logout()
        {
            _userService.Logout();
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
            var foundUser = _userService.Get(x => x.UserUsername == user.Username);

            ValidationResult result = loginValidator.Validate(user);

            if (result.IsValid)
            {
                if (_userService.LoginAdmin(user) || _userService.LoginWriter(user))
                {
                    if (foundUser.UserStatus)
                    {
                        return RedirectToAction("Index", "Homepage");
                    }
                    else
                    {
                        ModelState.AddModelError("Username","Hesabınız banlanmıştır , banın açılmasını bekleyebilir veya iletişim kısmından bilgi alabilirsiniz");
                        return View(user);
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("Password", "Şifreniz veya kullanıcı adınız yanlış");
                }

            }

            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }


            return View(user);
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
            ValidationResult result = registerValidator.Validate(entity);

            string extension, path;

            if (result.IsValid)
            {
                if (!entity.UserImage.IsNullOrWhiteSpace())
                {
                    extension = Path.GetExtension(Request.Files[0].FileName);

                    if (extension.Contains(".jpg") || extension.Contains(".png") || extension.Contains(".jpeg"))
                    {
                        System.IO.File.Delete(Server.MapPath("~") + entity.UserImage.Replace("~", ""));

                        extension = Path.GetExtension(Request.Files[0].FileName);

                        path = "~/wwwroot/profileImages/" + Guid.NewGuid() + extension;

                        Request.Files[0].SaveAs(Server.MapPath(path));

                        entity.UserImage = path.Replace("~", "");

                        _userService.Register(entity);
                        return RedirectToAction("Login", "Logins");
                    }
                    else
                    {
                        ModelState.AddModelError("UserImage",
                            "Kullanıcının resmi .jpg , .jpeg veya .png türünde olmalıdır");
                        
                    }

                    
                }

            }
            else
            {
                extension = Path.GetExtension(Request.Files[0].FileName);

                if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
                {

                }
                else
                {
                    ModelState.AddModelError("UserImage",
                        "Kullanıcının resmi .jpg , .jpeg veya .png türünde olmalıdır");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(entity);
        }


    }
}
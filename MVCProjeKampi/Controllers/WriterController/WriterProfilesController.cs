using System;
using System.IO;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

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

            var user = userService.Get(x => x.UserUsername == username);

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            var username = Session["Username"].ToString();

            ValidationResult results = validator.Validate(user);

            string extension, path;


            if (results.IsValid)
            {

                var foundUser = userService.Get(x => x.UserUsername == username);

                foundUser.UserFirstName = user.UserFirstName;
                foundUser.UserLastName = user.UserLastName;
                foundUser.UserAbout = user.UserAbout;
                foundUser.UserTitle = user.UserTitle;
                Session["Fullname"] = foundUser.UserFirstName + " " + foundUser.UserLastName;

                if (!user.UserImage.IsNullOrWhiteSpace())
                {

                    extension = Path.GetExtension(Request.Files[0].FileName);

                    if (extension.Contains(".jpg") || extension.Contains(".png") || extension.Contains(".jpeg"))
                    {
                        System.IO.File.Delete(Server.MapPath("~") + foundUser.UserImage.Replace("~", ""));

                        extension = Path.GetExtension(Request.Files[0].FileName);

                        path = "~/wwwroot/profileImages/" + Guid.NewGuid() + extension;

                        Request.Files[0].SaveAs(Server.MapPath(path));

                        foundUser.UserImage = path.Replace("~", "");

                        Session["UserImage"] = foundUser.UserImage;

                        userService.Update(foundUser);
                        return RedirectToAction("Index", "WriterHomepage");
                    }
                    else
                    {
                        ModelState.AddModelError("UserImage",
                            "Kullanıcının resmi .jpg , .jpeg veya .png türünde olmalıdır");

                        return View(user);
                    }


                }

                else
                {
                    userService.Update(foundUser);
                    return RedirectToAction("Index", "WriterHomepage");
                }


            }
            else
            {
                if (user.UserImage != null)
                {
                    extension = Path.GetExtension(Request.Files[0].FileName);

                    if (extension.Contains(".jpg") || extension.Contains(".png") || extension.Contains(".jpeg"))
                    {
                    }
                    else
                    {
                        ModelState.AddModelError("UserImage",
                            "Kullanıcının resmi .jpg , .jpeg veya .png türünde olmalıdır");
                    }
                }

                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }



            return View(user);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string password)
        {
            PasswordValidator registerValidator = new PasswordValidator();
            ValidationResult result = registerValidator.Validate(password);

            if (result.IsValid)
            {
                byte[] userPasswordHash, userPasswordSalt;

                var username = Session["Username"].ToString();
                var user = userService.Get(x => x.UserUsername == username);

                HashingHelper.CreatePasswordHash(password, out userPasswordHash, out userPasswordSalt);

                user.UserPasswordHash = userPasswordHash;
                user.UserPasswordSalt = userPasswordSalt;

                userService.Update(user);

                return RedirectToAction("Index", "WriterHomepage");

            }

            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }
    }
}
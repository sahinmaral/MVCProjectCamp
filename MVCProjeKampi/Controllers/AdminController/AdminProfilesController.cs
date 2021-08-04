using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using Microsoft.Ajax.Utilities;

using MVCProjeKampi.Models.ViewModels;

using System;
using System.IO;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminProfilesController : Controller
    {

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        private UserValidator validator = new UserValidator();
        public ActionResult Skills()
        {
            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());
            var skills = userService.GetUserSkills(user.UserId);

            ProfileSkillViewModel viewmodel = new ProfileSkillViewModel()
            {
                UserAbout = user.UserAbout,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserSkills = skills,
                UserImage = user.UserImage
            };

            return View(viewmodel);
        }


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

                    if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
                    {
                        System.IO.File.Delete(Server.MapPath("~") + foundUser.UserImage.Replace("~", ""));

                        path = "~/wwwroot/profileImages/" + Guid.NewGuid() + extension;

                        Request.Files[0].SaveAs(Server.MapPath(path));

                        foundUser.UserImage = path.Replace("~", "");

                        Session["UserImage"] = foundUser.UserImage;

                        userService.Update(foundUser);
                        return RedirectToAction("Index", "AdminHomepage");
                    }
                    else
                    {
                        ModelState.AddModelError("UserImage","Resminiz .jpg , .jpeg veya .png türünde olmalıdır");
                        return View(user);
                    }

                }
                else
                {
                    userService.Update(foundUser);
                    return RedirectToAction("Index", "AdminHomepage");
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
                    ModelState.AddModelError("UserImage", "Resminiz .jpg , .jpeg veya .png türünde olmalıdır");
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

                return RedirectToAction("Index", "AdminHomepage");

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
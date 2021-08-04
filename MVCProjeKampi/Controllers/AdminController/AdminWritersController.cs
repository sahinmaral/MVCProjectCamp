using System;
using System.IO;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using PagedList;

using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace MVCProjeKampi.Controllers.AdminController
{

    [Authorize(Roles = "Administrator")]
    public class AdminWritersController : Controller
    {
        private UserValidator writerValidator = new UserValidator();


        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));


        public ActionResult Index(int p = 1)
        {
            var username = Session["Username"].ToString();

            var foundUser = userService.Get(x => x.UserUsername == username);

            var writerValues = userService.GetList();

            writerValues.Remove(foundUser);

            var pagedWriterList = writerValues.ToPagedList(p, 8);

            return View(pagedWriterList);
        }

        [HttpGet]
        public ActionResult EditWriter(string username)
        {
            var user = userService.Get(x => x.UserUsername == username);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditWriter(User user)
        {
            var foundUser = userService.GetById(user.UserId);

            string path, extension;

            ValidationResult result = writerValidator.Validate(user);

            if (result.IsValid)
            {
                foundUser.UserLastName = user.UserLastName;
                foundUser.UserFirstName = user.UserFirstName;
                foundUser.UserAbout = user.UserAbout;
                foundUser.UserTitle = user.UserTitle;

                if (!user.UserImage.IsNullOrWhiteSpace())
                {
                    extension = Path.GetExtension(Request.Files[0].FileName);

                    if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
                    {
                        System.IO.File.Delete(Server.MapPath("~") + foundUser.UserImage.Replace("~", ""));

                        extension = Path.GetExtension(Request.Files[0].FileName);

                        path = "~/wwwroot/profileImages/" + Guid.NewGuid() + extension;

                        Request.Files[0].SaveAs(Server.MapPath(path));

                        foundUser.UserImage = path.Replace("~", "");

                        userService.Update(foundUser);
                        return RedirectToAction("Index", "AdminWriters");
                    }
                    else
                    {
                        ModelState.AddModelError("UserImage", "Resminiz .jpg , .jpeg veya .png türünde olmalıdır");

                        return View(user);
                    }

                }
                else
                {
                    userService.Update(foundUser);
                    return RedirectToAction("Index", "AdminWriters");
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

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(user);
        }

    }
}
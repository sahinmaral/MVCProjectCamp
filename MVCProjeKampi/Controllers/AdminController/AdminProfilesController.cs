﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System.Web.Mvc;
using BusinessLayer.ValidationRules;
using EntityLayer.Concrete;
using FluentValidation.Results;

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
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }

            return View(user);
        }


    }
}
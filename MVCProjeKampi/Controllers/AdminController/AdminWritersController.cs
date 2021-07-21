﻿using BusinessLayer.Abstract;
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
        private IWriterService writerManager = new WriterManager(new EfWriterDal(),new EfUserDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));


        public ActionResult Index(int p=1)
        {
            var WriterValues = writerManager.GetWriterDetails().ToPagedList(p,8);
            
            return View(WriterValues);
        }

        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AddWriter(Writer writer)
        {
            
            ValidationResult results = writerValidator.Validate(writer.User);
            if (results.IsValid)
            {
                writerManager.Add(writer);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditWriter(int id)
        {
            var user = userService.GetById(id);
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
                foundUser.UserEmail = user.UserEmail;
                foundUser.UserAbout = user.UserAbout;
                foundUser.UserTitle = user.UserTitle;
                foundUser.UserImage = user.UserImage;

                userService.Update(foundUser);

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


        public ActionResult BanUser(int userId,bool status)
        {
            var user = userService.GetById(userId);

            user.UserStatus = status;
            userService.Update(user);
            return RedirectToAction("Index","AdminAuthorization");
        }

    }
}
﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.DTOs;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class LoginsController : Controller
    {
        IUserService userService = new UserManager(new EfUserDal(),new EfSkillDal(),new RoleManager(new EfRoleDal(),new EfUserDal(),new EfUserRoleDal()));

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
            return RedirectToAction("Login", "Logins");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserForLoginDto user)
        {
            if (userService.LoginAdmin(user))
            {
                //İleriki zamanlarda bekleme süresi ekleyip yapılabilir
                //ViewBag.Message = "Başarılı bir şekilde giriş yaptınız";

                return RedirectToAction("Index", "AdminHomepage");
            }

            if (userService.LoginWriter(user))
            {
                return RedirectToAction("Index", "WriterHomepage");
            }

            ViewBag.Message = "Kullanıcı adı veya şifreniz yanlış";
            return View();
        }

    }
}
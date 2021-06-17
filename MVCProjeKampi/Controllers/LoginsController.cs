using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using EntityLayer.DTOs;

namespace MVCProjeKampi.Controllers
{
    public class LoginsController : Controller
    {
        IUserService userService = new UserManager(new EfUserDal(),new EfSkillDal());

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        //public ActionResult AdminLogout()
        //{
        //    return RedirectToAction("AdminLogin");
        //}

        [HttpPost]
        public ActionResult AdminLogin(UserForLoginDto admin)
        {
            if (userService.LoginByAdmin(admin))
            {
                //İleriki zamanlarda bekleme süresi ekleyip yapılabilir
                //ViewBag.Message = "Başarılı bir şekilde giriş yaptınız";

                return RedirectToAction("Index", "Categories");
            }

            
            ViewBag.Message = "Kullanıcı adı veya şifreniz yanlış";
            return View();
        }


    }
}
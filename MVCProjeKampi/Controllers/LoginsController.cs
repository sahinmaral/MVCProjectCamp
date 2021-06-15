using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class LoginsController : Controller
    {
        LoginManager loginManager = new LoginManager(new EfAdminDal(),new EfUserDal());

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Admin admin)
        {
            if (loginManager.LoginByAdmin(admin))
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
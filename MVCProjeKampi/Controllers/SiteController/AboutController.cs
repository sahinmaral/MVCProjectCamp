using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class AboutController : Controller
    {
        private IAboutService aboutService = new AboutManager(new EfAboutDal());
        public ActionResult Index()
        {
            var about = aboutService.GetList(x=>x.AboutStatus==true).First();
            return View(about);
        }
    }
}
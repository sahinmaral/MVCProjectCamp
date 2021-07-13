using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.WriterController
{
    public class WriterHomepageController : Controller
    {
        [Authorize(Roles = "Writer,User")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
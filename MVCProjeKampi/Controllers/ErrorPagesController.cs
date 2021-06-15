using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class ErrorPagesController : Controller
    {
        public ActionResult Page401()
        {
            Response.StatusCode = 401;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }
    }
}
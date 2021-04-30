using BusinessLayer.Concrete;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category

        CategoryManager categoryManager = new CategoryManager();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategoryList()
        {
            //var CategoryValues = categoryManager.GetAllBl();
            return View();
        }

        [HttpGet] 
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            //categoryManager.CategoryAddBl(category);
            return RedirectToAction("GetCategoryList");
        }
    }
}
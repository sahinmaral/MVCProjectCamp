using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System.Web.Mvc;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminCategoriesController : Controller
    {
        private IBaseService<Category> categoryManager = new CategoryManager(new EfCategoryDal());
        private CategoryValidator categoryValidator = new CategoryValidator();
        public ActionResult Index(int p=1)
        {
            var CategoryValues = categoryManager.GetList().ToPagedList(p,8);
            return View(CategoryValues);
        }



        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddCategory(Category p)
        {
            ValidationResult results = categoryValidator.Validate(p);

            if(results.IsValid)
            {
                categoryManager.Add(p);
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


        public ActionResult DeleteCategory(string categoryName)
        {
            var categoryValues = categoryManager.Get(x=>x.CategoryName == categoryName );
            categoryValues.CategoryStatus = false;
            categoryManager.Update(categoryValues);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateCategory(string categoryName)
        {
            var CategoryValues = categoryManager.Get(x=>x.CategoryName==categoryName);
            return View(CategoryValues);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            ValidationResult results = categoryValidator.Validate(category);

            if(results.IsValid)
            {
                categoryManager.Update(category);
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



    }
}
using System.Data.Entity.Infrastructure;
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
        private ICategoryService _categoryManager = new CategoryManager(new EfCategoryDal());

        private IHeadingService _headingService = new HeadingManager(new EfHeadingDal());

        private CategoryValidator categoryValidator = new CategoryValidator();
        public ActionResult Index(int p=1)
        {
            var CategoryValues = _categoryManager.GetList().ToPagedList(p,8);
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
                p.CategoryNameForFriendlyUrl = UrlSlugHelper.ToUrlSlug(p.CategoryName);
                _categoryManager.Add(p);
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


        public ActionResult DeleteCategory(string categoryNameForFriendlyUrl)
        {
            var category = _categoryManager.Get(x=>x.CategoryNameForFriendlyUrl == categoryNameForFriendlyUrl);
            category.CategoryStatus = false;

            _categoryManager.Update(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateCategory(string categoryNameForFriendlyUrl)
        {
            var CategoryValues = _categoryManager.Get(x=>x.CategoryNameForFriendlyUrl== categoryNameForFriendlyUrl);
            return View(CategoryValues);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            ValidationResult results = categoryValidator.Validate(category);

            if(results.IsValid)
            {
                category.CategoryNameForFriendlyUrl = UrlSlugHelper.ToUrlSlug(category.CategoryName);

                _categoryManager.Update(category);
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
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class CategoriesController : Controller
    {
        private IBaseService<Category> categoryManager = new CategoryManager(new EfCategoryDal());
        [Authorize(Roles = "Admin,Administrator,Moderator,QuestionAndAnswerTeam")]
        public ActionResult Index()
        {
            var CategoryValues = categoryManager.GetList();
            return View(CategoryValues);
        }
        [Authorize(Roles = "Admin,Administrator,Moderator")]
        public ActionResult EditCategories()
        {
            var CategoryValues = categoryManager.GetList();
            return View(CategoryValues);
        }

        [Authorize(Roles = "Admin,Administrator,Moderator")]
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Administrator,Moderator")]
        [HttpPost]
        public ActionResult AddCategory(Category p)
        {
            CategoryValidator categoryValidator = new CategoryValidator();
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

        [Authorize(Roles = "Admin,Administrator,Moderator")]
        public ActionResult DeleteCategory(int id)
        {
            var CategoryValues = categoryManager.GetById(id);
            categoryManager.Delete(CategoryValues);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Administrator,Moderator")]
        public ActionResult UpdateCategory(int id)
        {
            var CategoryValues = categoryManager.GetById(id);
            return View(CategoryValues);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Administrator,Moderator")]
        public ActionResult UpdateCategory(Category category)
        {
            CategoryValidator validationRules = new CategoryValidator();
            ValidationResult results = validationRules.Validate(category);

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
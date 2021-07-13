using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using FluentValidation.Results;

using System.Web.Mvc;
using System.Web.UI;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminWritersController : Controller
    {
        WriterValidator writerValidator = new WriterValidator();
        private IWriterService writerManager = new WriterManager(new EfWriterDal(),new EfUserDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Index()
        {
            var WriterValues = writerManager.GetWriterDetails();
            
            return View(WriterValues);
        }


        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult AddWriter(Writer writer)
        {
            
            ValidationResult results = writerValidator.Validate(writer);
            if (results.IsValid)
            {
                writerManager.Add(writer);
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

        [HttpGet]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult EditWriter(int id)
        {
            var WriterValue = writerManager.GetById(id);
            return View(WriterValue);
        }

        [HttpPost]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult EditWriter(Writer writer)
        {
            WriterValidator writerValidator = new WriterValidator();
            ValidationResult results = writerValidator.Validate(writer);
            if (results.IsValid)
            {
                var WriterValue = writerManager.Get(x=>x.WriterId==writer.WriterId);
                writerManager.Update(WriterValue);
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
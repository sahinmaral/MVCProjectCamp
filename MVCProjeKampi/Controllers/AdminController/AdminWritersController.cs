using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using FluentValidation.Results;

using System.Web.Mvc;
using System.Web.UI;
using EntityLayer.Concrete;
using PagedList;

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
        public ActionResult Index(int p=1)
        {
            var WriterValues = writerManager.GetWriterDetails().ToPagedList(p,8);
            
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
            var user = userService.GetById(id);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult EditWriter(User user)
        {
            var foundUser = userService.GetById(user.UserId);
            foundUser.UserUsername = user.UserUsername;
            foundUser.UserLastName = user.UserLastName;
            foundUser.UserFirstName = user.UserFirstName;
            foundUser.UserEmail = user.UserEmail;
            foundUser.UserAbout = user.UserAbout;
            foundUser.UserTitle = user.UserTitle;
            foundUser.UserImage = user.UserImage;

            userService.Update(foundUser);

            return RedirectToAction("Index");

           
        }
    }
}
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminMessagesController : Controller
    {
        private IMessageService messageManager = new MessageManager(new EfMessageDal(),
            new UserManager(new EfUserDal(), new EfSkillDal(),
                new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal())));
        private MessageValidator validator = new MessageValidator();

        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Inbox()
        {
            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor
            var messageValues = messageManager.GetListInboxToAdmin();
            return View(messageValues);
        }

        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Sendbox()
        {
            var messageValues = messageManager.GetListSendboxToAdmin();
            return View(messageValues);
        }

        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult GetMessageDetails(int id)
        {
            var contactValues = messageManager.GetById(id);
            contactValues.IsOpened = true;
            messageManager.Update(contactValues);
            return View(contactValues);
        }

        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }


        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            ValidationResult results = validator.Validate(message);
            if (results.IsValid)
            {
                messageManager.Add(message);
                return RedirectToAction("Index", "AdminContacts");
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
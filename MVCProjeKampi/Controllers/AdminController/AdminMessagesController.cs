using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminMessagesController : Controller
    {
        private IMessageService messageManager = new MessageManager(new EfMessageDal(),
           new UserManager(new EfUserDal(), new EfSkillDal(),
               new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal())));
        private MessageValidator validator = new MessageValidator();

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        public ActionResult Inbox()
        {
            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor
            var messageValues = messageManager.GetListInbox();
            return View(messageValues);
        }


        public ActionResult Sendbox()
        {
            var messageValues = messageManager.GetListSendbox();
            return View(messageValues);
        }


        public ActionResult GetMessageDetails(int id)
        {
            var contactValues = messageManager.GetById(id);
            contactValues.IsOpened = true;
            messageManager.Update(contactValues);
            return View(contactValues);
        }


        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }


        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            //İlerleyen zamanlarda eğer kullanı email güncelleyebilirse
            //kullanıcı id üzerinden yapmak zorunda

            var username = Session["Username"].ToString();

            var user = userService.Get(x => x.UserUsername == username);

            message.MessageDate = DateTime.Now;

            ValidationResult results = validator.Validate(message);
            if (results.IsValid)
            {
                message.SenderMail = user.UserEmail;
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

        public ActionResult SaveMessageToTheDraft(int id)
        {
            Message message = messageManager.GetById(id);
            message.IsDraft = true;
            messageManager.Update(message);
            return RedirectToAction("Index", "AdminContacts");
        }

    }
}
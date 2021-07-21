using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminMessagesController : Controller
    {
        private IMessageService messageService = new MessageManager(new EfMessageDal());
        private MessageValidator validator = new MessageValidator();

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        public ActionResult Inbox()
        {
            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor

            var username = Session["Username"].ToString();

            var user = userService.Get(x => x.UserUsername == username);

            var messages =  messageService.GetList(x => x.ReceiverMail == user.UserEmail);

            return View(messages);
        }

        public ActionResult Draft()
        {

            var username = Session["Username"].ToString();
            var user = userService.Get(x => x.UserUsername == username);

            var messageValues = messageService.GetList(x=>x.IsDraft==true);
            return View(messageValues);
        }

        public ActionResult Sendbox()
        {
            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            List<Message> messages = messageService.GetList(x => x.SenderMail == user.UserEmail);

            return View(messages);
        }


        public ActionResult GetMessageDetails(int id)
        {
            var contactValues = messageService.GetById(id);
            contactValues.IsOpened = true;
            messageService.Update(contactValues);
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
                messageService.Add(message);
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
            Message message = messageService.GetById(id);

            if (message.IsDraft)
            {
                message.IsDraft = false;
                messageService.Update(message);
                return RedirectToAction("Inbox", "AdminMessages");
            }
            else
            {
                message.IsDraft = true;
                messageService.Update(message);
                return RedirectToAction("Inbox", "AdminMessages");
            }
            
            
        }

    }
}
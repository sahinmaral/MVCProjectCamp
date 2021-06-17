using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers
{
    public class MessagesController : Controller
    {
        private IMessageService messageManager = new MessageManager(new EfMessageDal());
        private MessageValidator validator = new MessageValidator();

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Inbox()
        {
            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor
            var messageValues = messageManager.GetListInbox();
            return View(messageValues);
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Sendbox()
        {
            var messageValues = messageManager.GetListSendbox();
            return View(messageValues);
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult GetMessageDetails(int id)
        {
            var contactValues = messageManager.GetById(id);
            contactValues.IsOpened = true;
            messageManager.Update(contactValues);
            return View(contactValues);
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }


        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            ValidationResult results = validator.Validate(message);
            if (results.IsValid)
            {
                messageManager.Add(message);
                return RedirectToAction("Index","Contacts");
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
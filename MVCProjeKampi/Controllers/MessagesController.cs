using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers
{
    public class MessagesController : Controller
    {
        private MessageManager messageManager = new MessageManager(new EfMessageDal());
        private MessageValidator validator = new MessageValidator();
        [Authorize]
        public ActionResult Inbox()
        {
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
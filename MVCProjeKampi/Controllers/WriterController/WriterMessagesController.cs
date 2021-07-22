﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;
using PagedList;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer,User")]
    public class WriterMessagesController : Controller
    {
        private IMessageService messageService = new MessageManager(new EfMessageDal());
        private MessageValidator validator = new MessageValidator();

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        public ActionResult Inbox(int p=1)
        {
            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor

            var username = Session["Username"].ToString();

            var user = userService.Get(x => x.UserUsername == username);

            var messageValues = messageService.GetList(x => x.ReceiverMail == user.UserEmail).ToPagedList(p,10);
            return View(messageValues);
        }


        public ActionResult Sendbox(int p=1)
        {
            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var messages = messageService.GetList(x => x.SenderMail == user.UserEmail).ToPagedList(p,10);

            return View(messages);
        }

        public PartialViewResult GetContactSideMenu()
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.ArchiveCount = messageService.GetCount(x => x.IsArchived == true);
            viewModel.DraftCount = messageService.GetCount(x => x.IsDraft == true);
            viewModel.ReceivedMessageCount = messageService.GetCount(x => x.ReceiverMail == user.UserEmail && x.IsOpened == false);
            viewModel.SentMessageCount = messageService.GetCount(x => x.SenderMail == user.UserEmail && x.IsOpened == false);

            return PartialView(viewModel);
        }

        public ActionResult Archive(int p=1)
        {

            var username = Session["Username"].ToString();
            var user = userService.Get(x => x.UserUsername == username);

            var messageValues = messageService.GetList(x => x.IsArchived == true).ToPagedList(p,10);
            return View(messageValues);
        }

        //public ActionResult Draft()
        //{

        //    var username = Session["Username"].ToString();
        //    var user = userService.Get(x => x.UserUsername == username);

        //    var messageValues = messageService.GetList(x => x.IsDraft == true);
        //    return View(messageValues);
        //}

        public ActionResult SaveMessageToTheArchive(int id)
        {
            Message message = messageService.GetById(id);

            if (message.IsArchived)
            {
                message.IsArchived = false;
                messageService.Update(message);
            }
            else
            {
                message.IsArchived = true;
                messageService.Update(message);
            }

            //Sonra yapılacak
            //Hangi action üzerinden gönderiyorsam oraya göndersin
            return RedirectToAction("Archive", "WriterMessages");
        }


        //public ActionResult SaveMessageToTheDraft(Message message)
        //{
        //    //İlerleyen zamanlarda eğer kullanı email güncelleyebilirse
        //    //kullanıcı id üzerinden yapmak zorunda

        //    var username = Session["Username"].ToString();

        //    var user = userService.Get(x => x.UserUsername == username);

        //    message.MessageDate = DateTime.Now;

        //    ValidationResult results = validator.Validate(message);
        //    if (results.IsValid)
        //    {
        //        message.SenderMail = user.UserEmail;
        //        messageService.Add(message);
        //        return RedirectToAction("Draft", "WriterMessages");
        //    }
        //    else
        //    {
        //        foreach (var item in results.Errors)
        //        {
        //            ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
        //        }
        //    }

        //    return View("NewMessage");
        //}

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
                return RedirectToAction("Sendbox", "WriterMessages");
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
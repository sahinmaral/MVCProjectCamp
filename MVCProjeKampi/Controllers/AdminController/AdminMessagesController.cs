using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using PagedList;

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminMessagesController : Controller
    {
        private IMessageService _messageService = new MessageManager(new EfMessageDal());

        private IMessageStatusService _messageStatusService = new MessageStatusManager(new EfMessageStatusDal());

        private MessageValidator validator = new MessageValidator();

        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        public ActionResult Inbox(int p = 1)
        {

            var username = Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            var messages = _messageService.GetList(x => x.ReceiverUsername == user.UserUsername);

            var messageStatus = _messageStatusService.GetList(x=>x.UserId == user.UserId);

            foreach (var message in messages)
            {
                foreach (var messageStatu in messageStatus)
                {
                    if (messageStatu.MessageId == message.MessageId)
                    {
                        messageStatu.Message = message;
                    }
                }
            }

            List<MessageStatus> searchedMessageStatusList = new List<MessageStatus>();

            foreach (var messageStatu in messageStatus)
            {
                foreach (var message in messages)      
                {
                    if (messageStatu.MessageId == message.MessageId)
                    {
                        searchedMessageStatusList.Add(messageStatu);
                    }
                }
            }


            var messageStatusPagedList = searchedMessageStatusList.ToPagedList(p, 10);

            return View(messageStatusPagedList);
        }

        public ActionResult Archive(int p = 1)
        {

            var username = Session["Username"].ToString();
            var user = _userService.Get(x => x.UserUsername == username);

            var messages = _messageService.GetList();

            var messageStatus = _messageStatusService.GetList(x => x.UserId == user.UserId && x.IsArchived == true);

            foreach (var message in messages)
            {
                foreach (var messageStatu in messageStatus)
                {
                    if (messageStatu.MessageId == message.MessageId)
                    {
                        messageStatu.Message = message;
                    }
                }
            }

            var messageStatusPagedList = messageStatus.ToPagedList(p, 10);

            return View(messageStatusPagedList);
        }

        //public ActionResult Draft()
        //{

        //    var username = Session["Username"].ToString();
        //    var user = _userService.Get(x => x.UserUsername == username);

        //    var messageValues = _messageService.GetList(x => x.IsDraft == true);
        //    return View(messageValues);
        //}


        public ActionResult SaveMessageToTheArchive(int id)
        {
            var username = Session["Username"].ToString();
            var user = _userService.Get(x => x.UserUsername == username);

            var message = _messageService.GetById(id);

            var messageStatus =
                _messageStatusService.Get(x => x.MessageId == message.MessageId && x.UserId == user.UserId);

            if (messageStatus.IsArchived)
            {
                messageStatus.IsArchived = false;
                _messageStatusService.Update(messageStatus);
            }
            else
            {
                messageStatus.IsArchived = true;
                _messageStatusService.Update(messageStatus);
            }

            //Sonra yapılacak
            //Hangi action üzerinden gönderiyorsam oraya göndersin
            return RedirectToAction("Archive", "AdminMessages");
        }


        //public ActionResult SaveMessageToTheDraft(Message message)
        //{
        //    //İlerleyen zamanlarda eğer kullanı email güncelleyebilirse
        //    //kullanıcı id üzerinden yapmak zorunda

        //    var username = Session["Username"].ToString();

        //    var user = _userService.Get(x => x.UserUsername == username);

        //    message.MessageDate = DateTime.Now;

        //    ValidationResult results = validator.Validate(message);
        //    if (results.IsValid)
        //    {
        //        message.SenderMail = user.UserEmail;
        //        _messageService.Add(message);
        //        return RedirectToAction("Draft", "AdminMessages");
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

        public ActionResult Sendbox(int p=1)
        {
            var username = Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            var messages = _messageService.GetList(x => x.SenderUsername == user.UserUsername);

            var messageStatus = _messageStatusService.GetList(x => x.UserId == user.UserId);

            foreach (var message in messages)
            {
                foreach (var messageStatu in messageStatus)
                {
                    if (messageStatu.MessageId == message.MessageId)
                    {
                        messageStatu.Message = message;
                    }
                }
            }

            List<MessageStatus> searchedMessageStatusList = new List<MessageStatus>();

            foreach (var messageStatu in messageStatus)
            {
                foreach (var message in messages)
                {
                    if (messageStatu.MessageId == message.MessageId)
                    {
                        searchedMessageStatusList.Add(messageStatu);
                    }
                }
            }


            var messageStatusPagedList = searchedMessageStatusList.ToPagedList(p, 10);

            return View(messageStatusPagedList);
        }


        public ActionResult GetMessageDetails(int id)
        {
            var username = Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            var message = _messageService.Get(x => x.MessageId == id);

            var messageStatus =
                _messageStatusService.Get(x => x.MessageId == message.MessageId && x.UserId == user.UserId);

            messageStatus.IsOpened = true;

            _messageStatusService.Update(messageStatus);

            return View(message);
        }


        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }


        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            
            var username = Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            ValidationResult results = validator.Validate(message);
            if (results.IsValid)
            {
                message.MessageDate = DateTime.Now;
                message.SenderUsername = user.UserUsername;

                _messageService.Add(message);

                MessageStatus senderMessageStatus = new MessageStatus()
                {
                    MessageId = message.MessageId,
                    UserId = user.UserId
                };


                MessageStatus receiverMessageStatus = new MessageStatus()
                {
                    MessageId = message.MessageId,
                    UserId = _userService.Get(x => x.UserUsername == message.ReceiverUsername).UserId
                };

                _messageStatusService.Add(senderMessageStatus);
                _messageStatusService.Add(receiverMessageStatus);

                return RedirectToAction("Sendbox", "AdminMessages");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(message);
        }



    }
}
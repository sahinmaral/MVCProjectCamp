using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using EntityLayer.Concrete;
using FluentValidation.Results;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers.WriterController
{
    public class WriterMessagesController : Controller
    {
        private IMessageService messageService = new MessageManager(new EfMessageDal(),
            new UserManager(new EfUserDal(), new EfSkillDal(),
                new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal())));
        private MessageValidator validator = new MessageValidator();
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),new RoleManager(new EfRoleDal(),new EfUserDal(),new EfUserRoleDal()));


        [Authorize(Roles = "Writer,User")]
        public ActionResult Inbox()
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var email = user.UserEmail;

            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor

            var messageValues = messageService.GetList(x=>x.ReceiverMail == user.UserEmail);
            return View(messageValues);
        }

        [Authorize(Roles = "Writer,User")]
        public ActionResult Sendbox()
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var email = user.UserEmail;

            //Mesaja girdikten sonra sayfayı geri alınca yenilenmiyor
            //Sonrasında bakılması gerekiyor

            var messageValues = messageService.GetList(x => x.SenderMail == user.UserEmail);
            return View(messageValues);
        }

        [Authorize(Roles = "Writer,User")]
        public ActionResult GetMessageDetails(int id)
        {
            var contactValues = messageService.GetById(id);
            contactValues.IsOpened = true;
            messageService.Update(contactValues);
            return View(contactValues);
        }

        [Authorize(Roles = "Writer,User")]
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }


        [Authorize(Roles = "Writer,User")]
        [HttpPost]
        public ActionResult NewMessage(Message message)
        {
            ValidationResult results = validator.Validate(message);
            if (results.IsValid)
            {
                messageService.SendMessageUser(message);
                return RedirectToAction("Inbox", "WriterMessages");
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

        [Authorize(Roles = "Writer,User")]
        public PartialViewResult GetContactSideMenu()
        {
            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var test = messageService.GetList();


            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.ReceivedMessageCount = messageService.GetList().Count(x=>x.ReceiverMail==user.UserEmail && x.IsOpened == false);
            viewModel.SentMessageCount = messageService.GetList().Count(x => x.SenderMail == user.UserEmail);

            return PartialView(viewModel);
        }

    }
}
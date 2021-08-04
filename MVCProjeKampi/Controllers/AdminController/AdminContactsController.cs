using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using PagedList;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{

    [Authorize(Roles = "Administrator")]
    public class AdminContactsController : Controller
    {
        private IContactService _contactService = new ContactManager(new EfContactDal());
        private IMessageService _messageService = new MessageManager(new EfMessageDal());

        private IMessageStatusService _messageStatusService = new MessageStatusManager(new EfMessageStatusDal());

        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        public ActionResult Index(int p=1)
        {
            var contactValues = _contactService.GetList().ToPagedList(p,10);
            return View(contactValues);
        }


        public ActionResult GetContactDetails(int id)
        {
            var contactValues = _contactService.GetById(id);
            contactValues.IsOpened = true;
            _contactService.Update(contactValues);
            return View(contactValues);
        }


        public PartialViewResult GetContactSideMenu()
        {
            var username = Session["Username"];
            var user = _userService.Get(x => x.UserUsername == username.ToString());

            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();

            viewModel.ArchiveCount = _messageStatusService.GetList(x => x.UserId == user.UserId && x.IsArchived == true).Count;


            var receivedMessages = _messageService.GetList(x => x.ReceiverUsername == user.UserUsername);

            var senderMessages = _messageService.GetList(x => x.SenderUsername == user.UserUsername);




            var messageStatus = _messageStatusService.GetList(x => x.UserId == user.UserId && x.IsOpened == false);

            viewModel.ReceivedMessageCount = 0;

            foreach (var receivedMessage in receivedMessages)
            {
                foreach (var messageStatu in messageStatus)
                {
                    if (messageStatu.MessageId == receivedMessage.MessageId)
                    {
                        viewModel.ReceivedMessageCount++;
                    }
                }
            }


            viewModel.SentMessageCount = 0;

            foreach (var senderMessage in senderMessages)
            {
                foreach (var messageStatu in messageStatus)
                {
                    if (senderMessage.MessageId == messageStatu.MessageId)
                    {
                        viewModel.SentMessageCount++;
                    }
                }
            }
            
            return PartialView(viewModel);
        }

    }
}
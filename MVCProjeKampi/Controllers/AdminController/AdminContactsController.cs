using System.Linq;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System.Web.Mvc;
using System.Web.UI;
using BusinessLayer.Abstract;

namespace MVCProjeKampi.Controllers.AdminController
{

    [Authorize(Roles = "Administrator")]
    public class AdminContactsController : Controller
    {
        private IContactService contactService = new ContactManager(new EfContactDal());
        private IMessageService messageService = new MessageManager(new EfMessageDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        private ContactValidator contactValidator = new ContactValidator();

        public ActionResult Index()
        {
            var contactValues = contactService.GetList();
            return View(contactValues);
        }


        public ActionResult GetContactDetails(int id)
        {
            var contactValues = contactService.GetById(id);
            contactValues.IsOpened = true;
            contactService.Update(contactValues);
            return View(contactValues);
        }


        public PartialViewResult GetContactSideMenu()
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.DraftCount = messageService.GetCount(x => x.IsDraft == true);
            viewModel.ReceivedMessageCount = messageService.GetCount(x=>x.ReceiverMail == user.UserEmail && x.IsOpened == false);
            viewModel.SentMessageCount = messageService.GetCount(x=>x.SenderMail==user.UserEmail && x.IsOpened == false);
            viewModel.ContactCount = contactService.GetCount(x => x.IsOpened == false);

            return PartialView(viewModel);
        }

    }
}
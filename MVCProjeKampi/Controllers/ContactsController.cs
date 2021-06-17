using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using System.Web.Mvc;
using System.Web.UI;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers
{
    public class ContactsController : Controller
    {
        private ContactManager contactManager = new ContactManager(new EfContactDal());
        private MessageManager messageManager = new MessageManager(new EfMessageDal());

        private ContactValidator contactValidator = new ContactValidator();


        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Index()
        {
            var contactValues = contactManager.GetList();
            return View(contactValues);
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult GetContactDetails(int id)
        {
            var contactValues = contactManager.GetById(id);
            contactValues.IsOpened = true;
            contactManager.Update(contactValues);
            return View(contactValues);
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public PartialViewResult GetContactSideMenu()
        {
            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.ReceivedMessageCount = messageManager.GetListInbox().Count;
            viewModel.SentMessageCount = messageManager.GetListSendbox().Count;
            viewModel.ContactCount = contactManager.GetList(x=>x.IsOpened==false).Count;



            return PartialView(viewModel);
        }

    }
}
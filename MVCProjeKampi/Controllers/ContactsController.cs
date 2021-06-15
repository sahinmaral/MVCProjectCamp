using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers
{
    public class ContactsController : Controller
    {
        private ContactManager contactManager = new ContactManager(new EfContactDal());
        private MessageManager messageManager = new MessageManager(new EfMessageDal());

        private ContactValidator contactValidator = new ContactValidator();
        public ActionResult Index()
        {
            var contactValues = contactManager.GetList();
            return View(contactValues);
        }

        public ActionResult GetContactDetails(int id)
        {
            var contactValues = contactManager.GetById(id);
            return View(contactValues);
        }

        public PartialViewResult GetContactSideMenu()
        {
            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.ReceivedMessageCount = messageManager.GetListInbox().Count;
            viewModel.SentMessageCount = messageManager.GetListSendbox().Count;
            viewModel.ContactCount = contactManager.GetList().Count;



            return PartialView(viewModel);
        }

    }
}
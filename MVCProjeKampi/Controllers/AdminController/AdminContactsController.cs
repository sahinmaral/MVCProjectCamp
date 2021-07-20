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
        private ContactManager contactManager = new ContactManager(new EfContactDal());

        private IMessageService messageManager = new MessageManager(new EfMessageDal(),
            new UserManager(new EfUserDal(), new EfSkillDal(),
                new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal())));

        private ContactValidator contactValidator = new ContactValidator();

        public ActionResult Index()
        {
            var contactValues = contactManager.GetList();
            return View(contactValues);
        }


        public ActionResult GetContactDetails(int id)
        {
            var contactValues = contactManager.GetById(id);
            contactValues.IsOpened = true;
            contactManager.Update(contactValues);
            return View(contactValues);
        }


        public PartialViewResult GetContactSideMenu()
        {
            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.ReceivedMessageCount = messageManager.GetListInbox().Count(x=>x.IsOpened==false);
            viewModel.SentMessageCount = messageManager.GetListSendbox().Count(x=>x.IsOpened == false);
            viewModel.ContactCount = contactManager.GetList(x=>x.IsOpened==false).Count;

            return PartialView(viewModel);
        }

    }
}
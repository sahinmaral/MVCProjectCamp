using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System.Web.Mvc;
using BusinessLayer.Abstract;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminContactsController : Controller
    {
        private ContactManager contactManager = new ContactManager(new EfContactDal());

        private IMessageService messageManager = new MessageManager(new EfMessageDal(),
            new UserManager(new EfUserDal(), new EfSkillDal(),
                new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal())));

        private ContactValidator contactValidator = new ContactValidator();


        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Index()
        {
            var contactValues = contactManager.GetList();
            return View(contactValues);
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult GetContactDetails(int id)
        {
            var contactValues = contactManager.GetById(id);
            contactValues.IsOpened = true;
            contactManager.Update(contactValues);
            return View(contactValues);
        }

        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public PartialViewResult GetContactSideMenu()
        {
            CountOfMessagesViewModel viewModel = new CountOfMessagesViewModel();
            viewModel.ReceivedMessageCount = messageManager.GetListInboxToAdmin().Count;
            viewModel.SentMessageCount = messageManager.GetListSendboxToAdmin().Count;
            viewModel.ContactCount = contactManager.GetList(x=>x.IsOpened==false).Count;

            return PartialView(viewModel);
        }

    }
}
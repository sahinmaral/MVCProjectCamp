using System;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.SiteController
{
    [AllowAnonymous]
    public class ContactController : Controller
    {
        private ContactValidator validator = new ContactValidator();
        private IContactService _contactService = new ContactManager(new EfContactDal());

        [HttpGet]
        public ActionResult SendContact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendContact(Contact contact)
        {
            ValidationResult results = validator.Validate(contact);

            if (results.IsValid)
            {
                contact.ContactDate = DateTime.Now;
                _contactService.Add(contact);
                return RedirectToAction("","Homepage");
            }
            else
            {
                foreach (var items in results.Errors)
                {
                    ModelState.AddModelError(items.PropertyName,items.ErrorMessage);
                }

            }

            return View(contact);
        }
    }
}
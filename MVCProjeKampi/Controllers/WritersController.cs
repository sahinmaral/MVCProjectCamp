using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;

namespace MVCProjeKampi.Controllers
{
    public class WritersController : Controller
    {
        WriterValidator writerValidator = new WriterValidator();
        IWriterService writerManager = new WriterManager(new EfWriterDal(),new EfUserDal());

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        public ActionResult Index()
        {
            var WriterValues = writerManager.GetWriterDetails();
            
            return View(WriterValues);
        }

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddWriter(Writer writer)
        {
            
            ValidationResult results = writerValidator.Validate(writer);
            if (results.IsValid)
            {
                writerManager.Add(writer);
                return RedirectToAction("Index");
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

        [HttpGet]
        public ActionResult EditWriter(int id)
        {
            var WriterValue = writerManager.GetById(id);
            return View(WriterValue);
        }

        [HttpPost]
        public ActionResult EditWriter(Writer writer)
        {
            WriterValidator writerValidator = new WriterValidator();
            ValidationResult results = writerValidator.Validate(writer);
            if (results.IsValid)
            {
                writerManager.Update(writer);
                return RedirectToAction("Index");
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
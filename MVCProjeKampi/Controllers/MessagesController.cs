using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace MVCProjeKampi.Controllers
{
    public class MessagesController : Controller
    {
        private MessageManager messageManager = new MessageManager(new EfMessageDal());

        public ActionResult Inbox()
        {
            var messageValues = messageManager.GetList();
            return View(messageValues);
        }
    }
}
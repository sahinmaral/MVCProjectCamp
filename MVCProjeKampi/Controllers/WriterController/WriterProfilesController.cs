using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers.WriterController
{
    public class WriterProfilesController : Controller
    {
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        [HttpGet]
        [Authorize(Roles = "Writer,User")]
        public ActionResult EditProfile()
        {
            var username = Session["Username"].ToString();

            var writer = writerService.Get(x => x.User.UserUsername == username);

            foreach (var item in writerService.GetWriterDetails())
            {
                if (item.WriterId == writer.WriterId)
                {
                    writer = item;
                }
            }

            return View(writer);
        }

        [HttpPost]
        [Authorize(Roles = "Writer,User")]
        public ActionResult EditProfile(Writer writer)
        {
            var username = Session["Username"].ToString();

            var user = userService.Get(x => x.UserUsername == username);
            writer.User.UserUsername = username;
            writer.User.UserFirstName = user.UserFirstName;
            writer.User.UserLastName = user.UserLastName;
            writer.User.UserImage = user.UserImage;
            writer.User.UserAbout = user.UserAbout;
            writer.User.UserTitle = user.UserTitle;

            userService.Update(writer.User);

            return RedirectToAction("", "Homepage");
        }
    }
}
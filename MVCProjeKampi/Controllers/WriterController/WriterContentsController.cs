using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers.WriterController
{
    public class WriterContentsController : Controller
    {
        private IContentService contentManager = new ContentManager(new EfContentDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(), new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());

        [Authorize(Roles = "Writer,User")]
        public ActionResult ContentByHeading()
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var contentValues = contentManager.GetList(x=>x.Writer.UserId==user.UserId);

            return View(contentValues);
        }
    }
}
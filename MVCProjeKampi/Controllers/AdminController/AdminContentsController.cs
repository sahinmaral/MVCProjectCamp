﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System;
using System.Linq;
using System.Web.Mvc;
using EntityLayer.Concrete;
using PagedList;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminContentsController : Controller
    {
        private IContentService contentService = new ContentManager(new EfContentDal());

        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());

        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));


        public ActionResult ContentByHeading(int id)
        {

            var contents = contentService.GetList(x => x.HeadingId == id);

            foreach (var content in contents)
            {
                var writer = userService.GetById(content.UserId);
                content.User = userService.GetById(writer.UserId);
            }

            var heading = headingService.Get(x => x.HeadingId == id);


            ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel()
            {
                ContentList = contents.ToPagedList(1,10),
                Heading = heading
            };

            return View(viewModel);
        }


        public ActionResult MyContentByHeading(int p = 1)
        {
            var username = Session["Username"];
            var user = userService.Get(x => x.UserUsername == username.ToString());

            var contentValues = contentService.GetList(x => x.UserId == user.UserId).ToPagedList(p, 9);

            return View(contentValues);
        }


    }
}
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation.Results;

using MVCProjeKampi.Models.ViewModels;

using PagedList;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class HeadingsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));


        private ContentValidator validator = new ContentValidator();

        [AllowAnonymous]

        public ActionResult HeadingByHeadingId(int id, int p = 1)
        {
            var heading = headingService.GetById(id);
            var contents = contentService.GetList(x => x.HeadingId == id).ToPagedList(p, 8);
            var writers = writerService.GetWriterDetails();

            ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
            viewModel.ContentList = contents;
            viewModel.Heading = heading;

            foreach (var content in contents)
            {
                foreach (var writer in writers)
                {
                    if (content.WriterId == writer.WriterId)
                    {
                        content.Writer = writer;
                    }
                }
            }

            ViewBag.ContentsByHeadingViewModel = viewModel;

            return View();
        }

        [AllowAnonymous]
        public ActionResult HeadingByWriterId(int id, int p = 1)
        {
            var writer = writerService.GetById(id);

            var contents = contentService.GetList().
                OrderByDescending(x => x.ContentDate).Where(x => x.WriterId == id).ToPagedList(p, 8);

            ContentByWriterViewModel viewModel = new ContentByWriterViewModel();
            viewModel.Contents = contents;
            viewModel.Writer = writer;

            return View(viewModel);

        }

        [AllowAnonymous]
        public ActionResult HeadingByHeadingNameForFriendlyUrl(string headingNameForFriendlyUrl)
        {
            //Jeff Bezos'un astronot olması -> Jeff Bezos

            var heading = headingService.Get(x => x.HeadingNameForFriendlyUrl == headingNameForFriendlyUrl);

            return RedirectToAction("HeadingByHeadingId", "Headings", new { id = heading.HeadingId });
        }

        //[AllowAnonymous]
        //public ActionResult HeadingByHeadingName(string headingName,int p=1)
        //{

        //    İleride headingName için düzenleme yapılacak
        //    Mesela Doctor Who --> doctor-who

        //    var heading = headingService.Get(x=>x.HeadingName.StartsWith(headingName));
        //    var contents = contentService.GetList(x => x.HeadingId == heading.HeadingId).ToPagedList(p, 8);
        //    var writers = writerService.GetWriterDetails();

        //    ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
        //    viewModel.ContentList = contents;
        //    viewModel.Heading = heading;

        //    foreach (var content in contents)
        //    {
        //        foreach (var writer in writers)
        //        {
        //            if (content.WriterId == writer.WriterId)
        //            {
        //                content.Writer = writer;
        //            }
        //        }
        //    }

        //    ViewBag.ContentsByHeadingViewModel = viewModel;

        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult SearchHeadings(string searched)
        {
            //Search bar araştırılacak
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetHeadings()
        {
            var headings = headingService.GetList();

            List<string> headingNames = new List<string>(); 

            for (int i = 0; i < headings.Count; i++)
            {
                headingNames.Add(headings[i].HeadingName); 
            }

            return new JsonResult { Data = headingNames.ToArray(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult WriteContentOnHeading(Content content)
        {

            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var writer = writerService.Get(x => x.UserId == user.UserId);

            var heading = headingService.Get(x => x.HeadingId == content.Heading.HeadingId);

            content.Heading = heading;
            content.WriterId = writer.WriterId;
            content.HeadingId = heading.HeadingId;

            Content newContent = new Content();
            newContent.ContentDate = DateTime.Now;
            newContent.WriterId = content.WriterId;
            newContent.HeadingId = content.HeadingId;
            newContent.ContentText = content.ContentText;

            ValidationResult results = validator.Validate(newContent);

            if (results.IsValid)
            {
                contentService.Add(newContent);
                return RedirectToAction("HeadingByHeadingId", "Headings", new { id = content.Heading.HeadingId });
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                var refreshedHeading = headingService.GetById(content.HeadingId);
                var refreshedContents = contentService.GetList(x => x.HeadingId == content.HeadingId).ToPagedList(1, 8);
                var refreshedWriters = writerService.GetWriterDetails();

                ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
                viewModel.ContentList = refreshedContents;
                viewModel.Heading = heading;

                foreach (var refreshedContent in refreshedContents)
                {
                    foreach (var refreshedWriter in refreshedWriters)
                    {
                        if (refreshedContent.WriterId == refreshedWriter.WriterId)
                        {
                            refreshedContent.Writer = refreshedWriter;
                        }
                    }
                }

                ViewBag.ContentsByHeadingViewModel = viewModel;

            }

            return View("HeadingByHeadingId");

        }

    }
}
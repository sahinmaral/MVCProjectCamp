using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers.SiteController
{
    public class HeadingsController : Controller
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        private IContentService contentService = new ContentManager(new EfContentDal());
        private IWriterService writerService = new WriterManager(new EfWriterDal(), new EfUserDal());

        [AllowAnonymous]
        public ActionResult HeadingByHeadingId(int id)
        {
            var headingName = headingService.GetById(id).HeadingName;
            var contents = contentService.GetList(x => x.HeadingId == id);
            var writers = writerService.GetWriterDetails();

            ContentsByHeadingViewModel viewModel = new ContentsByHeadingViewModel();
            viewModel.ContentList = contents;
            viewModel.HeadingName = headingName;

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

            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult HeadingByWriterId(int id)
        {
            var writer = writerService.GetById(id);

            var contents = contentService.GetList().
                OrderByDescending(x => x.ContentDate).Where(x => x.WriterId == id).ToList();

            ContentByWriterViewModel viewModel = new ContentByWriterViewModel();
            viewModel.Contents = contents;
            viewModel.Writer = writer;

            return View(viewModel);

        }
    }
}
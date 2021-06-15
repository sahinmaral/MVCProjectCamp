using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System;
using System.Linq;
using System.Web.Mvc;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers
{
    public class ContentsController : Controller
    {
        private IBaseService<Content> contentManager = new ContentManager(new EfContentDal());
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentByHeading(int id)
        {
            string headingName = String.Empty;
            var contentValues = contentManager.GetList(x => x.HeadingId == id);
            var headingContent = contentValues.FirstOrDefault();
            if (headingContent != null)
            {
                headingName = headingContent.Heading.HeadingName;
            }

            ContentByHeadingViewModel model1 = new ContentByHeadingViewModel()
            {
                ContentList = contentValues,
                HeadingName = headingName
            };

            return View(model1);
        }


    }
}
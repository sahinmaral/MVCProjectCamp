using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using MVCProjeKampi.ViewModels;

using System;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers
{
    public class ContentController : Controller
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
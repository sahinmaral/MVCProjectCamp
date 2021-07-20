using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminGalleriesController : Controller
    {
        private IBaseService<ImageFile> _imageFileManager = new ImageFileManager(new EfImageFileDal());

        public ActionResult Index()
        {
            var images = _imageFileManager.GetList();
            return View(images);
        }
    }
}
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminHomepageController : Controller
    {
        private IImageFileService _imageFileService = new ImageFileManager(new EfImageFileDal());
        public ActionResult Index()
        {
            var images = _imageFileService.GetList();

            return View(images);
        }
    }
}
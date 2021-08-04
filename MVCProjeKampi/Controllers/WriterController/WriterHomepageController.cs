using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace MVCProjeKampi.Controllers.WriterController
{
    [Authorize(Roles = "Writer")]
    public class WriterHomepageController : Controller
    {
        private IImageFileService _imageFileService = new ImageFileManager(new EfImageFileDal());
        public ActionResult Index()
        {
            var images = _imageFileService.GetList();

            return View(images);
        }
    }
}
using System.Web.Mvc;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers
{
    public class GalleriesController : Controller
    {
        private IBaseService<ImageFile> _imageFileManager = new ImageFileManager(new EfImageFileDal());

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]

        public ActionResult Index()
        {
            var images = _imageFileManager.GetList();
            return View(images);
        }
    }
}
using System;
using System.IO;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using System.Web.Mvc;
using BusinessLayer.ValidationRules;
using FluentValidation.Results;
using Microsoft.Ajax.Utilities;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminGalleriesController : Controller
    {
        private IImageFileService _imageFileService = new ImageFileManager(new EfImageFileDal());
        private ImageFileValidator validator = new ImageFileValidator();

        public ActionResult Index()
        {
            var images = _imageFileService.GetList();
            return View(images);
        }

        public ActionResult DeleteImage(string imageNameForFriendlyUrl)
        {
            var image = _imageFileService.Get(x => x.ImageNameForFriendlyUrl == imageNameForFriendlyUrl);

            _imageFileService.Delete(image);

            System.IO.File.Delete(Server.MapPath("~")+image.ImagePath);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage(ImageFile imageFile)
        {
            ValidationResult results = validator.Validate(imageFile);

            string extension, path;

            if (results.IsValid)
            {
                if (!imageFile.ImagePath.IsNullOrWhiteSpace())
                {
                    extension = Path.GetExtension(Request.Files[0].FileName);

                    if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
                    {
                        System.IO.File.Delete(Server.MapPath("~") + imageFile.ImagePath.Replace("~", ""));

                        path = "~/wwwroot/galleries/" + Guid.NewGuid() + extension;

                        Request.Files[0].SaveAs(Server.MapPath(path));

                        imageFile.ImagePath = path.Replace("~", "");

                        imageFile.ImageNameForFriendlyUrl = UrlSlugHelper.ToUrlSlug(imageFile.ImageName);

                        _imageFileService.Add(imageFile);
                        return RedirectToAction("Index");

                    }

                    else
                    {
                        ModelState.AddModelError("ImagePath", "Resminiz .jpg , .jpeg veya .png türünde olmalıdır");
                        return View(imageFile);
                    }
                }

                
            }
            else
            {

                if (!imageFile.ImagePath.IsNullOrWhiteSpace())
                {
                    extension = Path.GetExtension(Request.Files[0].FileName);

                    if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
                    {
                       
                    }

                    else
                    {
                        ModelState.AddModelError("ImagePath", "Resminiz .jpg , .jpeg veya .png türünde olmalıdır");
                    }
                }

                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName,item.ErrorMessage);
                }
            }

            return View(imageFile);
        }

        [HttpGet]
        public ActionResult EditImage(string imageNameForFriendlyUrl)
        {
            var image = _imageFileService.Get(x => x.ImageNameForFriendlyUrl == imageNameForFriendlyUrl);

            return View(image);
        }

        [HttpPost]
        public ActionResult EditImage(ImageFile imageFile)
        {
            var foundImage = _imageFileService.GetById(imageFile.ImageId);

            ValidationResult results = validator.Validate(imageFile);

            string extension, path;

            if (results.IsValid)
            {
                if (!imageFile.ImagePath.IsNullOrWhiteSpace())
                {
                    System.IO.File.Delete(Server.MapPath("~") + imageFile.ImagePath.Replace("~", ""));

                    extension = Path.GetExtension(Request.Files[0].FileName);

                    path = "~/wwwroot/galleries/" + Guid.NewGuid() + extension;

                    Request.Files[0].SaveAs(Server.MapPath(path));

                    imageFile.ImagePath = path.Replace("~", "");

                    imageFile.ImageNameForFriendlyUrl = UrlSlugHelper.ToUrlSlug(imageFile.ImageName);

                    foundImage.ImageNameForFriendlyUrl = imageFile.ImageNameForFriendlyUrl;
                    foundImage.ImageName = imageFile.ImageName;
                    foundImage.ImagePath = imageFile.ImagePath;

                }

                _imageFileService.Update(foundImage);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(imageFile);
        }

    }
}
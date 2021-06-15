using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLayer.Concrete
{
    public class ImageFileManager : IImageFileService
    {
        private IImageFileDal _imageFileDal;
        public ImageFileManager(IImageFileDal imageFileDal)
        {
            _imageFileDal = imageFileDal;
        }

        public ImageFile GetById(int id)
        {
            return _imageFileDal.Get(x => x.ImageId == id);
        }

        public List<ImageFile> GetList()
        {
            return _imageFileDal.List();
        }

        public ImageFile Get(Expression<Func<ImageFile, bool>> filter)
        {
            return _imageFileDal.Get(filter);
        }

        public int GetCount()
        {
            List<ImageFile> imageFiles =  _imageFileDal.List();
            return imageFiles.Count();
        }

        public int GetCount(Expression<Func<ImageFile, bool>> filter)
        {
            List<ImageFile> imageFiles = _imageFileDal.List(filter);
            return imageFiles.Count();
        }

        public List<ImageFile> GetList(Expression<Func<ImageFile, bool>> filter)
        {
            return _imageFileDal.List(filter);
        }

        public void Add(ImageFile imageFile)
        {
            _imageFileDal.Insert(imageFile);
        }

        public void Delete(ImageFile imageFile)
        {
            _imageFileDal.Delete(imageFile);
        }

        public void Update(ImageFile imageFile)
        {
            _imageFileDal.Update(imageFile);
        }
    }
}

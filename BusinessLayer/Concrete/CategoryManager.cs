using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public void Add(Category category)
        {
            _categoryDal.Insert(category);
        }

        public void Delete(Category category)
        {
            _categoryDal.Delete(category);
        }

        public void Update(Category category)
        {
            _categoryDal.Update(category);
        }

        public Category Get(Expression<Func<Category, bool>> filter)
        {
            return _categoryDal.Get(filter);
        }

        public Category GetById(int id)
        {
            return _categoryDal.Get(x => x.CategoryId == id);
        }

        public List<Category> GetList(Expression<Func<Category, bool>> filter)
        {
            return _categoryDal.List(filter);
        }

        public int GetCount()
        {
            List<Category> categories = _categoryDal.List();
            return categories.Count();
        }

        public int GetCount(Expression<Func<Category, bool>> filter)
        {
            List<Category> categories = _categoryDal.List(filter);
            return categories.Count();
        }

        public List<Category> GetList()
        {
            return _categoryDal.List();
        }
    }
}

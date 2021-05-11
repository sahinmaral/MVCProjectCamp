using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService
    {
        List<Category> GetList();
        Category Get(Expression<Func<Category, bool>> filter);
        void Add(Category category);
        Category GetById(int id);
        void Delete(Category category);
        void Update(Category category);
        int GetCount();
        int GetCount(Expression<Func<Category, bool>> filter);
        List<Category> GetList(Expression<Func<Category, bool>> filter);
        
    }
}

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
    public class ContentManager : IContentService
    {
        IContentDal _contentDal;

        public ContentManager(IContentDal contentDal)
        {
            _contentDal = contentDal;
        }

        public void Add(Content content)
        {
            _contentDal.Insert(content);
        }

        public void Delete(Content content)
        {
            _contentDal.Delete(content);
        }

        public void Update(Content content)
        {
            _contentDal.Update(content);
        }

        public Content Get(Expression<Func<Content, bool>> filter)
        {
            return _contentDal.Get(filter);
        }

        public Content GetById(int id)
        {
            return _contentDal.Get(x => x.ContentId == id);
        }

        public List<Content> GetList(Expression<Func<Content, bool>> filter)
        {
            return _contentDal.List(filter);
        }

        public int GetCount()
        {
            return _contentDal.List().Count();
        }

        public int GetCount(Expression<Func<Content, bool>> filter)
        {
            return _contentDal.List(filter).Count();
        }

        public List<Content> GetList()
        {
            return _contentDal.List();
        }
    }
}

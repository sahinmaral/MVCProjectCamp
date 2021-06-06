using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class AboutManager : IAboutService
    {

        private IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public List<About> GetList()
        {
            return _aboutDal.List();
        }

        public About Get(Expression<Func<About, bool>> filter)
        {
            return _aboutDal.Get(filter);
        }

        public void Add(About entity)
        {
            _aboutDal.Insert(entity);
        }

        public About GetById(int id)
        {
            return _aboutDal.Get(x => x.AboutId == id);
        }

        public void Delete(About entity)
        {
            _aboutDal.Delete(entity);
        }

        public void Update(About entity)
        {
            _aboutDal.Update(entity);
        }

        public int GetCount()
        {
            return _aboutDal.List().Count;
        }

        public int GetCount(Expression<Func<About, bool>> filter)
        {
            return _aboutDal.List(filter).Count;
        }

        public List<About> GetList(Expression<Func<About, bool>> filter)
        {
            return _aboutDal.List(filter);
        }
    }
}

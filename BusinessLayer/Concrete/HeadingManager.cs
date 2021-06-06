using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class HeadingManager : IHeadingService
    {
        private IHeadingDal _headingDal;
        public HeadingManager(IHeadingDal headingDal)
        {
            _headingDal = headingDal;
        }

        public Heading Get(Expression<Func<Heading, bool>> filter)
        {
            return  _headingDal.Get(filter);
        }

        public Heading GetById(int id)
        {
            return _headingDal.Get(x => x.HeadingId == id);
        }

        public int GetCount()
        {
            return _headingDal.List().Count();
        }

        public int GetCount(Expression<Func<Heading, bool>> filter)
        {
            return _headingDal.List(filter).Count();
        }

        public List<Heading> GetList()
        {
            return _headingDal.List();
        }

        public List<Heading> GetList(Expression<Func<Heading, bool>> filter)
        {
            return _headingDal.List(filter);
        }

        public void Add(Heading heading)
        {
            _headingDal.Insert(heading);
        }

        public void Delete(Heading heading)
        {
            
            _headingDal.Update(heading);
        }

        public void Update(Heading heading)
        {
            _headingDal.Update(heading);
        }

    }
}

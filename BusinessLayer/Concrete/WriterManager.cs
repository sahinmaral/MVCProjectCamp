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
    public class WriterManager : IWriterService
    {
        private IWriterDal _writerDal;
        public WriterManager(IWriterDal writerDal)
        {
            _writerDal = writerDal;
        }

        public Writer GetById(int id)
        {
            return _writerDal.Get(x => x.WriterId == id);
        }

        public List<Writer> GetList()
        {
            return _writerDal.List();
        }

        public Writer Get(Expression<Func<Writer, bool>> filter)
        {
            return _writerDal.Get(filter);
        }

        public int GetCount()
        {
            List<Writer> writers =  _writerDal.List();
            return writers.Count();
        }

        public int GetCount(Expression<Func<Writer, bool>> filter)
        {
            List<Writer> writers = _writerDal.List(filter);
            return writers.Count();
        }

        public List<Writer> GetList(Expression<Func<Writer, bool>> filter)
        {
            return _writerDal.List(filter);
        }

        public void Add(Writer writer)
        {
            _writerDal.Insert(writer);
        }

        public void Delete(Writer writer)
        {
            _writerDal.Delete(writer);
        }

        public void Update(Writer writer)
        {
            _writerDal.Update(writer);
        }
    }
}

using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLayer.Concrete
{
    public class WriterManager : IWriterService
    {
        private IWriterDal _writerDal;
        private IUserDal _userDal;
        public WriterManager(IWriterDal writerDal,IUserDal userdal)
        {
            _writerDal = writerDal;
            _userDal = userdal;
        }

        public Writer GetById(int id)
        {
            var userValue = GetWriterDetails().Find(x => x.WriterId == id);
            var writer = _writerDal.Get(x => x.WriterId == id);
            writer.User = userValue.User;
            return writer;
        }

        public List<Writer> GetList()
        {
            return _writerDal.List();
        }

        public List<Writer> GetWriterDetails()
        {
            
            var users = _userDal.List();
            var writers = GetList();

            foreach (var writer in writers)
            {
                foreach (var user in users)
                {
                    if (user.UserId == writer.UserId)
                    {
                        writer.User = user;
                    }
                }
            }

            return writers;
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

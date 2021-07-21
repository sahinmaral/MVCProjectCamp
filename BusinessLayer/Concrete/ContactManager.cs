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
    public class ContactManager:IContactService
    {
        private IContactDal _contactDal;

        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        public List<Contact> GetList()
        {
            return _contactDal.List();
        }

        public Contact Get(Expression<Func<Contact, bool>> filter)
        {
            return _contactDal.Get(filter);
        }

        public void Add(Contact entity)
        { 
            _contactDal.Insert(entity);
        }

        public Contact GetById(int id)
        {
            return _contactDal.Get(x => x.ContactId == id);
        }

        public void Delete(Contact entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Contact entity)
        {
            _contactDal.Update(entity);
        }

        public int GetCount()
        {
            return _contactDal.List().Count;
        }

        public int GetCount(Expression<Func<Contact, bool>> filter)
        {
            return _contactDal.List(filter).Count;
        }

        public List<Contact> GetList(Expression<Func<Contact, bool>> filter)
        {
            return _contactDal.List(filter);
        }
    }
}

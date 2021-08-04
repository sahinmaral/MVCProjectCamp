using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace BusinessLayer.Concrete
{
    public class MessageStatusManager : IMessageStatusService
    {
        private IMessageStatusDal _messageStatusDal;

        public MessageStatusManager(IMessageStatusDal messageStatusDal)
        {
            _messageStatusDal = messageStatusDal;
        }

        public List<MessageStatus> GetList()
        {
            return _messageStatusDal.List();
        }

        public MessageStatus Get(Expression<Func<MessageStatus, bool>> filter)
        {
            return _messageStatusDal.Get(filter);
        }

        public void Add(MessageStatus entity)
        {
            _messageStatusDal.Insert(entity);
        }


        public MessageStatus GetById(int id)
        {
            return _messageStatusDal.Get(x => x.MessageStatusId == id);
        }

        public void Delete(MessageStatus entity)
        {
            _messageStatusDal.Delete(entity);
        }

        public void Update(MessageStatus entity)
        {
            _messageStatusDal.Update(entity);
        }

        public int GetCount()
        {
            return _messageStatusDal.List().Count;
        }

        public int GetCount(Expression<Func<MessageStatus, bool>> filter)
        {
            return _messageStatusDal.List(filter).Count;
        }

        public List<MessageStatus> GetList(Expression<Func<MessageStatus, bool>> filter)
        {
            return _messageStatusDal.List(filter);
        }
    }
}

using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace BusinessLayer.Concrete
{
    public class MessageManager : IMessageService
    {
        private IMessageDal _messageDal;
        private IUserService _userService;

        public MessageManager(IMessageDal messageDal,IUserService userService)
        {
            _messageDal = messageDal;
            _userService = userService;
        }

        public List<Message> GetList()
        {
            return _messageDal.List();
        }

        public List<Message> GetListInbox()
        {
            var username = HttpContext.Current.Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            return _messageDal.List(x => x.ReceiverMail == user.UserEmail);
        }

        public Message Get(Expression<Func<Message, bool>> filter)
        {
            return _messageDal.Get(filter);
        }

        public void Add(Message entity)
        { 
            _messageDal.Insert(entity);
        }

        public Message GetById(int id)
        {
            return _messageDal.Get(x => x.MessageId == id);
        }

        public void Delete(Message entity)
        {
            _messageDal.Delete(entity);
        }

        public void Update(Message entity)
        {
            _messageDal.Update(entity);
        }

        public int GetCount()
        {
            return _messageDal.List().Count;
        }

        public int GetCount(Expression<Func<Message, bool>> filter)
        {
            return _messageDal.List(filter).Count;
        }

        public List<Message> GetList(Expression<Func<Message, bool>> filter)
        {
            return _messageDal.List(filter);
        }

        public List<Message> GetListSendbox()
        {
            var username = HttpContext.Current.Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            return _messageDal.List(x => x.SenderMail == user.UserEmail );
        }
    }
}

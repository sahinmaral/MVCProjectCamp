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

        public List<Message> GetListInboxToAdmin()
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
            throw new NotImplementedException();
        }

        public void SendMessageAdmin(Message entity)
        {
            entity.MessageContent = entity.MessageContent.Replace("<p>", "");
            entity.MessageContent = entity.MessageContent.Replace("</p>", "");
            entity.SenderMail = "admin@gmail.com";
            entity.MessageDate = DateTime.Now;
            _messageDal.Insert(entity);
        }

        public void SendMessageUser(Message entity)
        {
            var username = HttpContext.Current.Session["Username"];

            var user = _userService.Get(x => x.UserUsername == username.ToString());

            entity.MessageContent = entity.MessageContent.Replace("<p>", "");
            entity.MessageContent = entity.MessageContent.Replace("</p>", "");

            entity.SenderMail = user.UserEmail;

            entity.MessageDate = DateTime.Now;
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

        public List<Message> GetListSendboxToAdmin()
        {
            var username = HttpContext.Current.Session["Username"].ToString();

            var user = _userService.Get(x => x.UserUsername == username);

            return _messageDal.List(x => x.SenderMail == user.UserEmail );
        }
    }
}

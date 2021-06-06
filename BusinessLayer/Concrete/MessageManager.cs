﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntitiesLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class MessageManager:IMessageService
    {
        private IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public List<Message> GetList()
        {
            return _messageDal.List(x=>x.ReceiverMail == "admin@gmail.com");
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
            return _messageDal.Get(x=>x.MessageId==id);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EntityLayer.Abstract;

namespace BusinessLayer.Abstract
{
    public interface IBaseService<TEntity>
    where TEntity : IEntity,new()
    {
        List<TEntity> GetList();
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        void Add(TEntity entity);
        TEntity GetById(int id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        int GetCount();
        int GetCount(Expression<Func<TEntity, bool>> filter);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> filter);
    }
}

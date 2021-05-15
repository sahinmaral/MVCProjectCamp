using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntitiesLayer.Abstract;
using EntityLayer.Concrete;

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

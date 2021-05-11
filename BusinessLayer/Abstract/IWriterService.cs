using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BusinessLayer.Abstract
{
    public interface IWriterService
    {
        List<Writer> GetList();
        void WriterAdd(Writer writer);
        Writer GetById(int id);
        void WriterDelete(Writer writer);
        void WriterUpdate(Writer writer);
        int GetSum();
        int GetSum(Expression<Func<Writer, bool>> filter);
    }
}

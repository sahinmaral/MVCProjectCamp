using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IHeadingService
    {
        List<Heading> GetList();
        Heading Get(Expression<Func<Heading, bool>> filter);
        List<Heading> GetList(Expression<Func<Heading, bool>> filter);
        void HeadingAdd(Heading writer);
        Heading GetById(int id);
        void HeadingDelete(Heading writer);
        void HeadingUpdate(Heading writer);
        int GetCount();
        int GetCount(Expression<Func<Heading,bool>> filter);
    }
}

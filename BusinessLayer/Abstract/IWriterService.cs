using EntityLayer.Concrete;

using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IWriterService:IBaseService<Writer>
    {
        List<Writer> GetWriterDetails();
    }
}

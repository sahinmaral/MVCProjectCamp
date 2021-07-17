using System.Collections.Generic;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IMessageService : IBaseService<Message>
    {
        List<Message> GetListSendbox();
        List<Message> GetListInbox();

    }
}

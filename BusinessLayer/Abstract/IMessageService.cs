using System.Collections.Generic;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IMessageService : IBaseService<Message>
    {
        List<Message> GetListSendboxToAdmin();
        List<Message> GetListInboxToAdmin();
        void SendMessageAdmin(Message entity);
        void SendMessageUser(Message entity);
    }
}

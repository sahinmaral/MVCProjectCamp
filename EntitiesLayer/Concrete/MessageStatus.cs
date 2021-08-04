using System.ComponentModel.DataAnnotations;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class MessageStatus : IEntity
    {
        [Key]
        public int MessageStatusId { get; set; }


        public int MessageId { get; set; }
        public virtual Message Message { get; set; }


        public int UserId { get; set; }


        public bool IsOpened { get; set; }
        public bool IsArchived { get; set; }

        
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Message : IEntity
    {
        public Message()
        {
            IsOpened = false;
            IsDraft = false;
        }

        [Key]
        public int MessageId { get; set; }


        [StringLength(50)]
        public string SenderMail { get; set; }


        [StringLength(50)]
        public string ReceiverMail { get; set; }


        [StringLength(100)]
        public string Subject { get; set; }


        [AllowHtml]
        public string MessageContent { get; set; }


        public DateTime MessageDate { get; set; }

        public bool IsOpened { get; set; }
        public bool IsDraft { get; set; }
    }
}

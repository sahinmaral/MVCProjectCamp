using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Message : IEntity
    {

        [Key]
        public int MessageId { get; set; }


        [StringLength(50)]
        public string SenderUsername { get; set; }


        [StringLength(50)]
        public string ReceiverUsername { get; set; }


        [StringLength(100)]
        public string Subject { get; set; }


        [AllowHtml]
        public string MessageContent { get; set; }

        public string PlainMessageContent { get; set; }

        public DateTime MessageDate { get; set; }

        public ICollection<MessageStatus> MessageStatus { get; set; }

    }
}

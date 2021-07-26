using EntityLayer.Abstract;

using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Content : IEntity
    {
        [Key]
        public int ContentId { get; set; }
        [StringLength(1000)]
        public string ContentText { get; set; }
        public DateTime ContentDate { get; set; }

        public int HeadingId { get; set; }
        public virtual Heading Heading { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }


    }
}

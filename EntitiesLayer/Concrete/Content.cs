using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        [StringLength(1000)]
        public string ContentText { get; set; }
        public DateTime ContentDate { get; set; }

        public int HeaderId { get; set; }
        public virtual Heading Heading { get; set; }

        public Writer WriterId { get; set; }
        public virtual Writer Writer { get; set; }

    }
}

using EntityLayer.Abstract;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Concrete
{
    public class Writer : IEntity
    {
        [Key]
        public int WriterId { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }


        public ICollection<Heading> Headings { get; set; }
        public ICollection<Content> Contents { get; set; }

        


    }
}

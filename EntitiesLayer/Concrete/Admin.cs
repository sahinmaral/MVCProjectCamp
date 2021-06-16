using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Admin : IEntity
    {
        [Key]
        public int AdminId { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

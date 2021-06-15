using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Admin: IEntity
    {
        [Key , ForeignKey("User")] 
        public int AdminId { get; set; }
        public User User { get; set; }
    }
}

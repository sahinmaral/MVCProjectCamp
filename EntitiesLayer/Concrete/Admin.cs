using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        [StringLength(50)]
        public string AdminUsername { get; set; }
        [StringLength(50)]
        public string AdminPassword { get; set; }
        [StringLength(1)]
        public string AdminRole { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(50)]
        public string UserUsername { get; set; }

        [StringLength(50)]
        public string UserPassword { get; set; }

        public ICollection<UserRole> UserRoles;
        public ICollection<Skill> Skills;

        public Writer Writer { get; set; }
        public Admin Admin { get; set; }
    }
}

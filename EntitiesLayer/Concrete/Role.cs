using EntityLayer.Abstract;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Role:IEntity
    {
        [Key]
        public int RoleId { get; set; }
        [StringLength(50)]
        public string RoleName { get; set; }

        public ICollection<UserRole> UserRoles;
    }
}

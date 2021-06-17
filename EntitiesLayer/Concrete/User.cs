using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class User : IEntity
    {
        [Key]
        public int UserId { get; set; }


        [StringLength(50)]
        public string UserUsername { get; set; }


        [StringLength(50)]
        public string UserFirstName { get; set; }


        [StringLength(50)]
        public string UserLastName { get; set; }


        [StringLength(500)]
        public byte[] UserPasswordHash { get; set; }

        [StringLength(500)]
        public byte[] UserPasswordSalt { get; set; }


        [StringLength(300)]
        public string UserImage { get; set; }


        [StringLength(100)]
        public string UserEmail { get; set; }


        [StringLength(200)]
        public string UserAbout { get; set; }


        [StringLength(50)]
        public string UserTitle { get; set; }


        [DefaultValue("true")]
        public bool UserStatus { get; set; }

        public ICollection<UserRole> UserRoles;
        public ICollection<Skill> Skills;

        public ICollection<Writer> Writers { get; set; }
        public ICollection<Admin> Admins { get; set; }
    }
}

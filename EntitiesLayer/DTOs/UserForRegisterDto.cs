using EntityLayer.Concrete;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.DTOs
{
    public class UserForRegisterDto
    {

        [StringLength(50)]
        public string UserUsername { get; set; }


        [StringLength(50)]
        public string UserFirstName { get; set; }


        [StringLength(50)]
        public string UserLastName { get; set; }


        [MaxLength(500)]
        public string UserPassword { get; set; }


        [StringLength(300)]
        public string UserImage { get; set; }


        [StringLength(100)]
        public string UserEmail { get; set; }


        [StringLength(200)]
        public string UserAbout { get; set; }


        [StringLength(50)]
        public string UserTitle { get; set; }

    }
}

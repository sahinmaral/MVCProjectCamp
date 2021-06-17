using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Models.ViewModels
{
    public class ProfileSkillViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserAbout { get; set; }
        public List<Skill> UserSkills { get; set; }
    }
}
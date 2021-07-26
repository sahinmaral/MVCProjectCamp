using EntityLayer.Concrete;

using System.Collections.Generic;

namespace MVCProjeKampi.Models.ViewModels
{
    public class ProfileSkillViewModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserAbout { get; set; }
        public string UserImage { get; set; }
        public List<Skill> UserSkills { get; set; }
    }
}
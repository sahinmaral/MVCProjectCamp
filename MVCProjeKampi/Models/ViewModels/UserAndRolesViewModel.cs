using System.Collections.Generic;
using System.Web.Mvc;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Models.ViewModels
{
    public class UserAndRolesViewModel
    {
        public User User { get; set; }
        public List<SelectedRoleViewModel> Roles { get; set; }
    }
}
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    public class AdminProfilesController : Controller
    {
        IUserService _userService = new UserManager(new EfUserDal(),new EfSkillDal(),new RoleManager(new EfRoleDal(),new EfUserDal(),new EfUserRoleDal()));

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var username = Session["Username"];

            var user = _userService.Get(x => x.UserUsername.Equals(username));
            var skills = _userService.GetUserSkills(user.UserId);

            ProfileSkillViewModel viewmodel = new ProfileSkillViewModel()
            {
                UserAbout = user.UserAbout,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserSkills = skills
            };

            return View(viewmodel);
        }
    }
}
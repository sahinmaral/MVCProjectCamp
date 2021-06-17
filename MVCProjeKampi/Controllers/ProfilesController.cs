using System.Web.Mvc;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using MVCProjeKampi.Models.ViewModels;

namespace MVCProjeKampi.Controllers
{
    public class ProfilesController : Controller
    {
        IUserService _userService = new UserManager(new EfUserDal(),new EfSkillDal());

        [Authorize(Roles = "Admin,User")]
        [Authorize(Roles = "Administrator,User")]
        [Authorize(Roles = "Moderator,User")]
        [Authorize(Roles = "QuestionAndAnswerTeam,User")]
        public ActionResult AdminAndOtherProfiles()
        {
            var username = Session["Username"];

            var user = _userService.Get(x => x.UserUsername == username);
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
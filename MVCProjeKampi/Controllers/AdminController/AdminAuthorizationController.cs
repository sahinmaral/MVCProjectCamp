using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using MVCProjeKampi.Models.ViewModels;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminAuthorizationController : Controller
    {
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        private IRoleService roleService = new RoleManager(new EfRoleDal(),
            new EfUserDal(), new EfUserRoleDal());

        public ActionResult Index()
        {
            var users = userService.GetList();

            List<UserAndRolesViewModel> viewmodel = new List<UserAndRolesViewModel>();

            foreach (var user in users)
            {
                viewmodel.Add(new UserAndRolesViewModel()
                {
                    User = user,
                    //UserRoles = roleService.GetRolesForUser(user.UserUsername)
            });

                
            }

            return View(viewmodel);
        }


        [HttpGet]
        public ActionResult GiveAuthorization(int userId)
        {
            var user = userService.GetById(userId);

            var userRoles = roleService.GetRolesForUser(user.UserUsername);
            var allRoles = roleService.GetList();

            List<SelectedRoleViewModel> selectedRoleViewModels = new List<SelectedRoleViewModel>();

            foreach (var role in allRoles)
            {
                selectedRoleViewModels.Add(new SelectedRoleViewModel()
                {
                    Role = role
                });
            }

            foreach (var selectedViewModel in selectedRoleViewModels)
            {
                foreach (var userRole in userRoles)
                {
                    if (selectedViewModel.Role.RoleName == userRole)
                    {
                        selectedViewModel.IsSelected = true;
                    }
                }
            }

            UserAndRolesViewModel viewModel = new UserAndRolesViewModel();
            viewModel.Roles = selectedRoleViewModels;
            viewModel.User = user;

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult GiveAuthorization(UserAndRolesViewModel viewModel)
        {
            var user = userService.GetById(viewModel.User.UserId);

            foreach (var role in viewModel.Roles)
            {
                if (role.IsSelected)
                {
                    var selectedRole = roleService.GetById(role.Role.RoleId);
                    UserRole item = new UserRole()
                    {
                        RoleId = selectedRole.RoleId,
                        UserId = user.UserId
                    };

                    roleService.GiveRoleToUser(item);
                }
                
            }

            return RedirectToAction("Index", "AdminAuthorization");
        }
    }
}
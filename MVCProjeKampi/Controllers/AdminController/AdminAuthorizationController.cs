using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using MVCProjeKampi.Models.ViewModels;

using PagedList;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCProjeKampi.Controllers.AdminController
{
    [Authorize(Roles = "Administrator")]
    public class AdminAuthorizationController : Controller
    {
        private IUserService userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(), new EfUserDal(), new EfUserRoleDal()));

        private IRoleService roleService = new RoleManager(new EfRoleDal(),
            new EfUserDal(), new EfUserRoleDal());


        public ActionResult Index(int p = 1)
        {
            var username = Session["Username"];

            var user = userService.Get(x => x.UserUsername == username.ToString());

            var users = userService.GetList();

            users.Remove(user);

            var pagedListUsers = users.ToPagedList(p, 8);

            return View(pagedListUsers);
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
        public ActionResult GiveAuthorization(UserAndRolesViewModel viewModel, FormCollection frm)
        {
            var user = userService.GetById(viewModel.User.UserId);

            int roleId = int.Parse(frm["Roles"]);

            var selectedRole = roleService.GetById(roleId);

            var roles = roleService.GetRolesForUser(user.UserUsername).ToList();

            var userRole = roles.Find(x => x == "User");

            roles.Remove(userRole);

            foreach (var role in roles)
            {
                int deletedRole = roleService.Get(x => x.RoleName == role).RoleId;

                var deletedUserRole = roleService.GetUserRole(user.UserId, deletedRole);
                roleService.DeleteRoleFromUser(deletedUserRole);
            }

            roleService.GiveRoleFromUser(new UserRole()
            {
                RoleId = selectedRole.RoleId,
                UserId = user.UserId
            });


            return RedirectToAction("Index", "AdminAuthorization");
        }
    }
}
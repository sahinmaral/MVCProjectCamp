using System.Collections.Generic;
using System.Globalization;
using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System.Linq;
using System.Web;
using System.Web.Security;
using BusinessLayer.Abstract;


namespace BusinessLayer.Concrete
{
    public class RoleManager:IRoleService
    {
        IRoleDal _roleDal;
        IUserDal _userDal;
        IUserRoleDal _userRoleDal;
        public RoleManager(IRoleDal roleDal,IUserDal userDal,IUserRoleDal userRoleDal)
        {
            _roleDal = roleDal;
            _userDal = userDal;
            _userRoleDal = userRoleDal;
        }


        public string[] GetRolesForUser(string username)
        {
            List<string> currentUserRoles = new List<string>();
            
            var user = _userDal.List().FirstOrDefault(x => x.UserUsername == username);

            var userRoles = _userRoleDal.List(x => x.UserId == user.UserId);

            var ok = _roleDal.Get(x => x.RoleId == 5);

            foreach (var userRole in userRoles)
            {
                var ok2 = _roleDal.Get(x => x.RoleId == userRole.RoleId).RoleName;
                currentUserRoles.Add(_roleDal.Get(x => x.RoleId == userRole.RoleId).RoleName);
            }


            return currentUserRoles.ToArray();

        }


    }
}

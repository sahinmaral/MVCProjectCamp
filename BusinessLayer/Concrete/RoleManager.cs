using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace BusinessLayer.Concrete
{
    public class RoleManager : IRoleService
    {
        IRoleDal _roleDal;
        IUserDal _userDal;
        IUserRoleDal _userRoleDal;
        public RoleManager(IRoleDal roleDal, IUserDal userDal, IUserRoleDal userRoleDal)
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

        public void GiveRoleToUser(UserRole entity)
        {
            _userRoleDal.Insert(entity);
        }

        public List<Role> GetList()
        {
            return _roleDal.List();
        }

        public Role Get(Expression<Func<Role, bool>> filter)
        {
            return _roleDal.Get(filter);
        }

        public void Add(Role entity)
        {
            _roleDal.Insert(entity);
        }

        public Role GetById(int id)
        {
            return _roleDal.Get(x => x.RoleId == id);
        }

        public void Delete(Role entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Role entity)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            return _roleDal.List().Count();
        }

        public int GetCount(Expression<Func<Role, bool>> filter)
        {
            return _roleDal.List(filter).Count();
        }

        public List<Role> GetList(Expression<Func<Role, bool>> filter)
        {
            return _roleDal.List(filter);
        }
    }
}

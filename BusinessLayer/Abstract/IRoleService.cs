using EntityLayer.Concrete;

using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IRoleService:IBaseService<Role>
    {
        string[] GetRolesForUser(string username);
        void GiveRoleFromUser(UserRole entity);
        void DeleteRoleFromUser(UserRole entity);
        UserRole GetUserRole(int userId,int roleId);
    }
}
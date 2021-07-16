using EntityLayer.Concrete;

using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IRoleService:IBaseService<Role>
    {
        string[] GetRolesForUser(string username);
        void GiveRoleToUser(UserRole entity);
    }
}
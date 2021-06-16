using System.Collections.Generic;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IRoleService
    {
        string[] GetRolesForUser(string username);
    }
}
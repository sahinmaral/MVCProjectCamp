using EntityLayer.Concrete;
using EntityLayer.DTOs;

using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IUserService: IBaseService<User>
    {
        bool LoginAdmin(UserForLoginDto admin);
        bool LoginWriter(UserForLoginDto writer);
        List<Skill> GetUserSkills(int userId);
        void Logout();
    }
}
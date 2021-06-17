using System.Collections.Generic;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;

namespace BusinessLayer.Concrete
{
    public interface IUserService: IBaseService<User>
    {
        bool LoginByAdmin(UserForLoginDto admin);
        List<Skill> GetUserSkills(int userId);
    }
}
using DataAccessLayer.Abstract;

using EntityLayer.Concrete;
using EntityLayer.DTOs;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;


namespace BusinessLayer.Concrete
{
    public class UserManager:IUserService
    {
        
        private IUserDal _userDal;
        private ISkillDal _skillDal;

        public UserManager(IUserDal userDal,ISkillDal skillDal)
        {
            _userDal = userDal;
            _skillDal = skillDal;
        }

        public bool LoginByAdmin(UserForLoginDto admin)
        {

            var userToCheck = _userDal.Get(x => x.UserUsername==admin.Username);

            if (userToCheck == null)
            {
                return false;
            }

            if (!HashingHelper.VerifyPasswordHash(admin.Password, userToCheck.UserPasswordHash,
                userToCheck.UserPasswordSalt))
            {
                return false;
            }

            FormsAuthentication.SetAuthCookie(admin.Username, false);
            HttpContext.Current.Session["Username"] = admin.Username;
            HttpContext.Current.Session["UserImage"] = userToCheck.UserImage;
            HttpContext.Current.Session["Fullname"] = userToCheck.UserFirstName + " " + userToCheck.UserLastName;
            return true;


        }

        public List<Skill> GetUserSkills(int userId)
        {
            return _skillDal.List(x => x.UserId == userId);
        }

        public List<User> GetList()
        {
            throw new NotImplementedException();
        }

        public User Get(Expression<Func<User, bool>> filter)
        {
            return _userDal.Get(filter);
        }

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return _userDal.Get(x => x.UserId == id);
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<User> GetList(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}

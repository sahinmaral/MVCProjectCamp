using BusinessLayer.Abstract;

using DataAccessLayer.Abstract;

using EntityLayer.Concrete;
using EntityLayer.DTOs;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Security;


namespace BusinessLayer.Concrete
{
    public class UserManager:IUserService
    {
        
        private IUserDal _userDal;
        private ISkillDal _skillDal;
        private IRoleService _roleService;
        public UserManager(IUserDal userDal,ISkillDal skillDal,IRoleService roleService)
        {
            _userDal = userDal;
            _skillDal = skillDal;
            _roleService = roleService;
        }

        public bool LoginAdmin(UserForLoginDto admin)
        {

            var userToCheck = _userDal.Get(x => x.UserUsername==admin.Username);

            var roles = _roleService.GetRolesForUser(admin.Username);

            if (userToCheck == null)
            {
                return false;
            }

            if (!HashingHelper.VerifyPasswordHash(admin.Password, userToCheck.UserPasswordHash,
                userToCheck.UserPasswordSalt))
            {
                return false;
            }

            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i].Contains("Administrator"))
                {
                    FormsAuthentication.SetAuthCookie(admin.Username, false);
                    HttpContext.Current.Session["Username"] = admin.Username;
                    HttpContext.Current.Session["UserImage"] = userToCheck.UserImage;
                    HttpContext.Current.Session["Fullname"] = userToCheck.UserFirstName + " " + userToCheck.UserLastName;
                    return true;
                }
            }

            return false;
        }

        public bool LoginWriter(UserForLoginDto writer)
        {
            var userToCheck = _userDal.Get(x => x.UserUsername == writer.Username);

            if (userToCheck == null)
            {
                return false;
            }

            if (!HashingHelper.VerifyPasswordHash(writer.Password, userToCheck.UserPasswordHash,
                userToCheck.UserPasswordSalt))
            {
                return false;
            }

            FormsAuthentication.SetAuthCookie(writer.Username, false);
            HttpContext.Current.Session["Username"] = writer.Username;
            HttpContext.Current.Session["UserImage"] = userToCheck.UserImage;
            HttpContext.Current.Session["Fullname"] = userToCheck.UserFirstName + " " + userToCheck.UserLastName;
            return true;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.RedirectToLoginPage();
        }

        public void Register(UserForRegisterDto entity)
        {
            byte[] passwordSalt, passwordHash;

            HashingHelper.CreatePasswordHash(entity.UserPassword, out passwordHash, out passwordSalt);

            User user = new User()
            {
                UserAbout = entity.UserAbout,
                UserEmail = entity.UserEmail,
                UserUsername = entity.UserUsername,
                UserFirstName = entity.UserFirstName,
                UserImage = entity.UserImage,
                UserLastName = entity.UserLastName,
                UserTitle = entity.UserTitle,
                UserPasswordHash = passwordHash,
                UserPasswordSalt = passwordSalt
            };

            Add(user);

            _roleService.GiveRoleFromUser(new UserRole()
            {
                UserId = user.UserId,
                RoleId = 5
            });

            _roleService.GiveRoleFromUser(new UserRole()
            {
                UserId = user.UserId,
                RoleId = 6
            });


        }
        

        public List<Skill> GetUserSkills(int userId)
        {
            return _skillDal.List(x => x.UserId == userId);
        }

        public List<User> GetList()
        {
            return _userDal.List();
        }

        public User Get(Expression<Func<User, bool>> filter)
        {
            return _userDal.Get(filter);
        }

        public void Add(User entity)
        {
            _userDal.Insert(entity);
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
            _userDal.Update(entity);
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<User, bool>> filter)
        {
            return _userDal.List(filter).Count;
        }

        public List<User> GetList(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}

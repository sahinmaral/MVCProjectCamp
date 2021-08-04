using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.DTOs;

using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));
        public UserForLoginDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifrenizi girmeniz gerekiyor");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adınızı girmeniz gerekiyor");

            RuleFor(x => x.Username).Must(x => IsUserExisted(x))
                .WithMessage("Böyle bir kullanıcı adında olan kullanıcı bulunamadı");

        }

        public bool IsUserExisted(string username)
        {
            var user = _userService.Get(x => x.UserUsername == username);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}

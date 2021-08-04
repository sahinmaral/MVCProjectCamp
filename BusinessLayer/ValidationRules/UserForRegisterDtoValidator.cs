using System.Text.RegularExpressions;
using System.Web.UI;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using EntityLayer.DTOs;

using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
    {
        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        public UserForRegisterDtoValidator()
        {
            RuleFor(x => x.UserFirstName).NotEmpty().WithMessage("Adı boş geçemezsiniz");
            RuleFor(x => x.UserLastName).NotEmpty().WithMessage("Soyadını boş geçemezsiniz");
            RuleFor(x => x.UserTitle).NotEmpty().WithMessage("Ünvan kısmını boş geçemezsiniz");
            RuleFor(x => x.UserAbout).NotEmpty().WithMessage("Hakkında kısmını boş geçemezsiniz");

            RuleFor(x => x.UserEmail).NotEmpty().WithMessage("Email kısmı boş bırakılamaz");
            RuleFor(x => x.UserEmail).Must(x => IsEmailTaken(x)).WithMessage("Böyle bir email zaten mevcuttur");

            RuleFor(x => x.UserUsername).NotEmpty().WithMessage("Kullanıcı adını boş bırakamazsınız");
            
            RuleFor(x => x.UserUsername).Must(x => IsUserExisted(x)).WithMessage("Böyle bir kullanıcı zaten mevcuttur");


            RuleFor(x => x.UserImage).NotEmpty().WithMessage("Resim yüklemeniz gerekiyor");

            RuleFor(x=>x.UserEmail).EmailAddress().WithMessage("Geçersiz bir email adresidir");

            RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Şifre kısmını boş geçemezsiniz");
            RuleFor(x => x.UserPassword).Must(x=> PasswordRegex(x)).WithMessage(
                "Şifrenizin en az 8 karakter olması ve bir büyük harf , bir küçük harf , bir rakam ve bir özel karakter olması zorundadır.");

            RuleFor(x => x.UserFirstName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserPassword).MinimumLength(8).WithMessage("Lütfen en az 8 karakter girişi yapın");
            RuleFor(x => x.UserUsername).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserLastName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserTitle).MinimumLength(3).WithMessage("Lütfen en az 3 karakter girişi yapın");
            RuleFor(x => x.UserAbout).MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapın");

            RuleFor(x => x.UserFirstName).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserUsername).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserLastName).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserAbout).MaximumLength(200).WithMessage("Lütfen en fazla 200 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserTitle).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserAbout).MaximumLength(200).WithMessage("Lütfen en fazla 200 karakterden fazla değer girişi yapmayın");

            
            //Görev olarak verilen validation
            //RuleFor(x => x.UserAbout).Must(x=>x.Contains('a')).WithMessage("Yazar hakkında a harfi geçmesi gerekir");
        }

        private bool PasswordRegex(string password)
        {
            Regex regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (password!=null)
            {
                if (regex.IsMatch(password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public bool IsUserExisted(string username)
        {
            var user = _userService.Get(x => x.UserUsername == username);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsEmailTaken(string email)
        {
            var user = _userService.Get(x => x.UserEmail == email);

            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}

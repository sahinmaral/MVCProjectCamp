using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator:AbstractValidator<Message>
    {
        private IUserService _userService = new UserManager(new EfUserDal(), new EfSkillDal(),
            new RoleManager(new EfRoleDal(),
                new EfUserDal(), new EfUserRoleDal()));

        public MessageValidator()
        {
            RuleFor(x => x.ReceiverUsername).NotEmpty().WithMessage("Alıcının kullanıcı adını boş geçemezsiniz");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Konuyu boş geçemezsiniz");
            RuleFor(x => x.MessageContent).NotEmpty().WithMessage("Mesajı boş geçemezsiniz");

            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Lütfen en az 3 karakter girişi yapın");
            RuleFor(x => x.Subject).MaximumLength(100).WithMessage("Lütfen en fazla 100 karakter girişi yapın");

            RuleFor(x => x.ReceiverUsername).Must(x => IsUsernameValid(x))
                .WithMessage("Emaili göndereceğiniz kullanıcı sistemde kayıtlı değildir");
        }

        public bool IsUsernameValid(string username)
        {
            var user = _userService.Get(x => x.UserUsername == username);

            if (user==null)
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

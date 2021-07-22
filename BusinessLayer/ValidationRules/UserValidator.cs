using EntityLayer.Concrete;

using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserFirstName).NotEmpty().WithMessage("Yazar adını boş geçemezsiniz");
            RuleFor(x => x.UserLastName).NotEmpty().WithMessage("Yazar soyadını boş geçemezsiniz");
            RuleFor(x => x.UserTitle).NotEmpty().WithMessage("Ünvan kısmını boş geçemezsiniz");
            RuleFor(x => x.UserAbout).NotEmpty().WithMessage("Hakkında kısmını boş geçemezsiniz");


            RuleFor(x => x.UserFirstName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserUsername).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserLastName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserTitle).MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapın");
            RuleFor(x => x.UserAbout).MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapın");

            RuleFor(x => x.UserFirstName).MaximumLength(50).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.UserUsername).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserLastName).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserAbout).MaximumLength(200).WithMessage("Lütfen en fazla 200 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.UserTitle).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");

            //Görev olarak verilen validation
            //RuleFor(x => x.UserAbout).Must(x=>x.Contains('a')).WithMessage("Yazar hakkında a harfi geçmesi gerekir");
        }
    }
}

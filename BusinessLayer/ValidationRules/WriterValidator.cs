using EntityLayer.Concrete;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class WriterValidator:AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.User.UserFirstName).NotEmpty().WithMessage("Yazar adını boş geçemezsiniz");
            RuleFor(x => x.User.UserLastName).NotEmpty().WithMessage("Yazar soyadını boş geçemezsiniz");
            RuleFor(x => x.User.UserTitle).NotEmpty().WithMessage("Ünvan kısmını boş geçemezsiniz");
            RuleFor(x => x.User.UserAbout).NotEmpty().WithMessage("Hakkında kısmını boş geçemezsiniz");
            RuleFor(x => x.User.UserUsername).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.User.UserLastName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.User.UserUsername).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.User.UserLastName).MaximumLength(20).WithMessage("Lütfen en fazla 50 karakterden fazla değer girişi yapmayın");
            RuleFor(x => x.User.UserAbout).Must(x=>x.Contains('a')).WithMessage("Yazar hakkında a harfi geçmesi gerekir");
        }
    }
}

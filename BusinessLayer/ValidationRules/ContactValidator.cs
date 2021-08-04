using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ContactValidator:AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage("Email boş bırakılamaz");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Konu boş bırakılamaz");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Mesaj boş bırakılamaz");


            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Lütfen en az 3 karakter giriş yapınız");
            RuleFor(x => x.Message).MinimumLength(10).WithMessage("Lütfen en az 10 karakter giriş yapınız");
            RuleFor(x => x.UserEmail).MinimumLength(3).WithMessage("Lütfen en az 3 karakter giriş yapınız");

            RuleFor(x => x.Subject).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakter girişi yapınız");
            RuleFor(x => x.UserEmail).MaximumLength(50).WithMessage("Lütfen en fazla 50 karakter giriş yapınız");
            RuleFor(x => x.Message).MaximumLength(1000).WithMessage("Lütfen en fazla 1000 karakter giriş yapınız");
            

            RuleFor(x => x.UserEmail).EmailAddress().WithMessage("Geçersiz bir email adresidir");

        }
    }
}

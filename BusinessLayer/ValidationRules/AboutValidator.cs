using BusinessLayer.Abstract;
using BusinessLayer.Concrete;

using DataAccessLayer.EntityFramework;

using EntityLayer.Concrete;

using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class AboutValidator:AbstractValidator<About>
    {
        private IAboutService _aboutService = new AboutManager(new EfAboutDal());
        public AboutValidator()
        {
            RuleFor(x => x.AboutHeader).NotEmpty().WithMessage("Hakkında başlığını boş geçemezsiniz");
            RuleFor(x => x.AboutText).NotEmpty().WithMessage("Hakkında yazısını boş geçemezsiniz");


            RuleFor(x => x.AboutHeader).MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapın");
            RuleFor(x => x.AboutText).MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapın");


            RuleFor(x => x.AboutText).MaximumLength(1000).WithMessage("Lütfen en fazla 1000 karakter girişi yapın");
            RuleFor(x => x.AboutHeader).MaximumLength(30).WithMessage("Lütfen en fazla 30 karakter girişi yapın");


        }

    }
}

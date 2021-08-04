using EntityLayer.Concrete;

using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ImageFileValidator:AbstractValidator<ImageFile>
    {
        public ImageFileValidator()
        {
            RuleFor(x => x.ImageName).NotEmpty().WithMessage("Resmin adını boş geçemezsiniz");
            RuleFor(x => x.ImagePath).NotEmpty().WithMessage("Resim yüklemeniz gerekir");

            RuleFor(x => x.ImageName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapın");
            RuleFor(x => x.ImagePath).MinimumLength(10).WithMessage("Lütfen en az 10 karakter girişi yapın");

            RuleFor(x => x.ImageName).MaximumLength(100).WithMessage("Lütfen en fazla 100 karakter girişi yapın");
            RuleFor(x => x.ImagePath).MaximumLength(250).WithMessage("Lütfen en fazla 250 karakter girişi yapın");
        }
    }
}

using EntityLayer.Concrete;

using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class ContentValidator:AbstractValidator<Content>
    {
        public ContentValidator()
        {
            RuleFor(x => x.ContentText).MinimumLength(5).WithMessage("Yazınız en az 5 karakterden oluşmalıdır");
            RuleFor(x => x.ContentText).MaximumLength(5000).WithMessage("Yazınız en fazla 5000 karakterden oluşmalıdır");
            RuleFor(x => x.ContentText).NotEmpty().WithMessage("Yazınızı boş geçemezsiniz");
        }
    }
}

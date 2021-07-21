using EntityLayer.Concrete;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class HeadingValidator:AbstractValidator<Heading>
    {
        public HeadingValidator()
        {
            RuleFor(x => x.HeadingName).NotEmpty().WithMessage("Başlık adını boş geçemezsiniz");
            RuleFor(x => x.HeadingName).MaximumLength(30).WithMessage("Başlık adı en çok 30 karakter olmalıdır");
            RuleFor(x => x.HeadingName).MinimumLength(5).WithMessage("Başlık adı en az 5 karakter olmalıdır");
        }
    }
}

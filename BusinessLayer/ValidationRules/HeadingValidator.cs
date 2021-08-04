using EntityLayer.Concrete;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;

namespace BusinessLayer.ValidationRules
{
    public class HeadingValidator:AbstractValidator<Heading>
    {
        private IHeadingService headingService = new HeadingManager(new EfHeadingDal());
        public HeadingValidator()
        {
            RuleFor(x => x.HeadingName).NotEmpty().WithMessage("Başlık adını boş geçemezsiniz");


            RuleFor(x => x.HeadingName).MaximumLength(100).WithMessage("Başlık adı en çok 100 karakter olmalıdır");
            RuleFor(x => x.HeadingName).MinimumLength(5).WithMessage("Başlık adı en az 5 karakter olmalıdır");


            RuleFor(x => x.HeadingName).Must(CheckIfHeadingNameExisted).WithMessage("Aynı başlık adı olamaz");
        }

        private bool CheckIfHeadingNameExisted(string headingName)
        {
            var heading = headingService.Get(x => x.HeadingName == headingName);
            if (heading == null)
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

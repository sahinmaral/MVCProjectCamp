using System.Text.RegularExpressions;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class PasswordValidator:AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("Şifre kısmını boş geçemezsiniz");
            RuleFor(x => x).Must(x => PasswordRegex(x)).WithMessage(
                "Şifrenizin en az 8 karakter olması ve bir büyük harf , bir küçük harf , bir rakam ve bir özel karakter olması zorundadır.");
            RuleFor(x => x).MinimumLength(8).WithMessage("Lütfen en az 8 karakter girişi yapın");
        }

        private bool PasswordRegex(string password)
        {
            Regex regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            if (password != null)
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
    }
}

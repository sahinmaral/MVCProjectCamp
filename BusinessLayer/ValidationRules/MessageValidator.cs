using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator:AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.ReceiverMail).NotEmpty().WithMessage("Alıcı adresini boş geçemezsiniz");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Konuyu boş geçemezsiniz");
            RuleFor(x => x.MessageContent).NotEmpty().WithMessage("Mesajı boş geçemezsiniz");

            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Lütfen en az 3 karakter girişi yapın");
            RuleFor(x => x.Subject).MaximumLength(100).WithMessage("Lütfen en fazla 100 karakter girişi yapın");


            RuleFor(x => x.ReceiverMail).EmailAddress().WithMessage("Geçersiz bir email adresidir");
            RuleFor(x => x.SenderMail).EmailAddress().WithMessage("Geçersiz bir email adresidir");
        }
    }
}

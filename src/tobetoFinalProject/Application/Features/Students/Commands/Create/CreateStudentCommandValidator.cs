using FluentValidation;

namespace Application.Features.Students.Commands.Create;

public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("��renci ad� bo� olamaz.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("��renci soyad� bo� olamaz.");

        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Ge�erli bir e-posta adresi giriniz.");

        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("�ifre en az 6 karakter uzunlu�unda olmal�d�r.");
    }
}
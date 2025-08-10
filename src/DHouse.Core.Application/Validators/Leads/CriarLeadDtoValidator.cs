using FluentValidation;
using DHouse.Core.Application.DTOs.Leads;

namespace DHouse.Core.Application.Validators.Leads;

public class CriarLeadDtoValidator : AbstractValidator<CriarLeadDto>
{
    public CriarLeadDtoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Telefone).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Telefone));
    }
}

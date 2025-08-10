using FluentValidation;
using DHouse.Core.Application.DTOs.Imoveis;

namespace DHouse.Core.Application.Validators.Imoveis;

public class CriarImovelDtoValidator : AbstractValidator<CriarImovelDto>
{
    public CriarImovelDtoValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(160);
        RuleFor(x => x.Preco).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Endereco).NotNull();
        RuleFor(x => x.Endereco.Cidade).NotEmpty();
        RuleFor(x => x.Endereco.Estado).NotEmpty().Length(2, 2);
        RuleFor(x => x.Endereco.Cep).NotEmpty().Length(8, 9);
    }
}

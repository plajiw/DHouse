using DHouse.Core.ValueObjects;
using FluentValidation;

namespace DHouse.Core.Validators.ValueObjects
{
    public class EnderecoValidator : AbstractValidator<Endereco>
    {
        public EnderecoValidator() 
        {
            RuleFor(e => e.Logradouro).MaximumLength(200).WithMessage("O logradouro deve ter no máximo 200 caracteres.");
            RuleFor(e => e.Numero).MaximumLength(20).WithMessage("O número deve ter no máximo 20 caracteres.");
            RuleFor(e => e.Bairro).MaximumLength(100).WithMessage("O bairro deve ter no máximo 100 caracteres.");
            RuleFor(e => e.Cidade).MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres.");
            RuleFor(e => e.Complemento).MaximumLength(150).WithMessage("O complemento deve ter no máximo 150 caracteres.");
            RuleFor(e => e.CEP).Matches(@"^\d{5}-\d{3}$|^\d{8}$")
                .WithMessage("O CEP deve estar no formato 00000-000 ou conter 8 dígitos.")
                .When(e => !string.IsNullOrEmpty(e.CEP)); // Só executa quando o CEP não é nulo/vazio
        }
    }
}

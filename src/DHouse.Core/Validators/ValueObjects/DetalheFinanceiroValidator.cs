using DHouse.Core.ValueObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHouse.Core.Validators.ValueObjects
{
    public class DetalheFinanceiroValidator : AbstractValidator<DetalheFinanceiro>
    {
        public DetalheFinanceiroValidator()
        {
            RuleFor(df => df)
                .Must(df => df.ValorVenda.HasValue || df.ValorAluguel.HasValue)
                .WithMessage("O imóvel precisa ter um valor de venda ou de aluguel definido.");

            RuleFor(df => df.ValorVenda)
                .GreaterThan(0).WithMessage("O valor de venda deve ser maior que zero.")
                .When(df => df.ValorVenda.HasValue);

            RuleFor(df => df.ValorAluguel)
                .GreaterThan(0).WithMessage("O valor do aluguel deve ser maior que zero.")
                .When(df => df.ValorAluguel.HasValue);

            RuleFor(df => df.ValorCondominioMensal)
                .GreaterThanOrEqualTo(0).WithMessage("O valor do condomínio não pode ser negativo.")
                .When(df => df.ValorCondominioMensal.HasValue);

            RuleFor(df => df.ValorIPTUMensal)
                .GreaterThanOrEqualTo(0).WithMessage("O valor do IPTU não pode ser negativo.")
                .When(df => df.ValorIPTUMensal.HasValue);

            RuleFor(df => df.ValorVenda)
                .NotNull().WithMessage("É necessário informar o valor de venda para aceitar financiamento.")
                .When(df => df.AceitaFinanciamento);

            RuleFor(df => df.ValorVenda)
                .NotNull().WithMessage("É necessário informar o valor de venda para aceitar permuta.")
                .When(df => df.AceitaPermuta);
        }
    }
}

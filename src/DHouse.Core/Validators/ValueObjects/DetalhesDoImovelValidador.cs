using DHouse.Core.ValueObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHouse.Core.Validators.ValueObjects
{
    public class DetalhesDoImovelValidador : AbstractValidator<DetalhesDoImovel>
    {
        public DetalhesDoImovelValidador()
        {
            RuleFor(d => d.AreaTotal).GreaterThan(0).When(d => d.AreaTotal.HasValue);
            RuleFor(d => d.AreaUtil).GreaterThan(0).When(d => d.AreaUtil.HasValue);
            RuleFor(d => d.Quartos).GreaterThanOrEqualTo(0).When(d => d.Quartos.HasValue);
            RuleFor(d => d.Suites).GreaterThanOrEqualTo(0).When(d => d.Suites.HasValue);
            RuleFor(d => d.Banheiros).GreaterThanOrEqualTo(0).When(d => d.Banheiros.HasValue);
            RuleFor(d => d.Lavabos).GreaterThanOrEqualTo(0).When(d => d.Lavabos.HasValue);
            RuleFor(d => d.VagasGaragemTotal).GreaterThanOrEqualTo(0).When(d => d.VagasGaragemTotal.HasValue);
            RuleFor(d => d.VagasGaragemCobertas).GreaterThanOrEqualTo(0).When(d => d.VagasGaragemCobertas.HasValue);
            RuleFor(d => d.AnoDeConstrucao)
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage($"O ano de construção não pode ser no futuro (estamos em {DateTime.UtcNow.Year}).")
                .When(d => d.AnoDeConstrucao.HasValue);

            RuleFor(d => d.AreaUtil)
                .LessThanOrEqualTo(d => d.AreaTotal)
                .WithMessage("A área útil não pode ser maior que a área total.")
                .When(d => d.AreaUtil.HasValue && d.AreaTotal.HasValue);

            RuleFor(d => d.Suites)
                .LessThanOrEqualTo(d => d.Quartos)
                .WithMessage("O número de suítes não pode ser maior que o número total de quartos.")
                .When(d => d.Suites.HasValue && d.Quartos.HasValue);

            RuleFor(d => d.VagasGaragemCobertas)
                .LessThanOrEqualTo(d => d.VagasGaragemTotal)
                .WithMessage("O número de vagas cobertas não pode ser maior que o total de vagas.")
                .When(d => d.VagasGaragemCobertas.HasValue && d.VagasGaragemTotal.HasValue);

            RuleFor(d => d.Andar)
                .LessThanOrEqualTo(d => d.TotalDeAndaresNoPredio)
                .WithMessage("O andar do apartamento não pode ser maior que o total de andares do prédio.")
                .When(d => d.Andar.HasValue && d.TotalDeAndaresNoPredio.HasValue);
    }
    }
}

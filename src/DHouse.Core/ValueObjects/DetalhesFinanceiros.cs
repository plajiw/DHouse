namespace DHouse.Core.ValueObjects
{
    public record DetalheFinanceiro(
        decimal? ValorVenda,
        decimal? ValorAluguel,
        decimal? ValorCondominioMensal,
        decimal? ValorIPTUMensal,
        bool AceitaFinanciamento = false,
        bool AceitaPermuta = false,
        IReadOnlyList<TipoGarantiaLocacao>? GarantiasAceitas = null,
        decimal? ValorSeguroIncendioAnual = null
    );
}

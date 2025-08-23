namespace DHouse.Core.ValueObjects
{
    public record DetalhesPrivativos(
        decimal? AreaTotal,
        decimal? AreaUtil,
        int? AnoDeConstrucao,
        int? Andar,
        int? TotalDeAndaresNoPredio,
        int? Quartos,
        int? Suites,
        int? Banheiros,
        int? Lavabos,
        int? VagasGaragemTotal,
        int? VagasGaragemCobertas,
        bool? PossuiEscaninho = false,
        bool? Mobiliado = false,
        bool? ArmariosNaCozinha = false,
        bool? ArmariosNosQuartos = false,
        bool? Varanda = false,
        bool? VarandaGourmet = false
    );
}
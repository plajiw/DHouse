using DHouse.Core.Enums;

namespace DHouse.Core.DTOs
{
    public class CriteriosDeBuscaDeImoveis
    {
        public string? TermoDeBusca { get; set; }
        public string? CodigoDoImovel { get; set; }
        public List<TipoDeImovel>? Tipos { get; set; }
        public string? Cidade { get; set; }
        public List<string>? Bairros { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
        public decimal? ValorMaximoCondominio { get; set; }
        public bool? AceitaFinanciamento { get; set; }
        public bool? AceitaPermuta { get; set; }
        public decimal? AreaUtilMinima { get; set; }
        public int? Quartos { get; set; }
        public int? SuitesMinimas { get; set; }
        public int? BanheirosMinimos { get; set; }
        public int? VagasGaragemMinimas { get; set; }
        public int? AnoConstrucaoMinimo { get; set; }
        public bool? Mobiliado { get; set; }
        public bool? ComVarandaGourmet { get; set; }
        public bool? ComPiscina { get; set; }
        public bool? ComAcademia { get; set; }
        public bool? ComPortaria24Horas { get; set; }
        public bool? PermiteAnimais { get; set; }
        public bool? ComElevador { get; set; }
    }
}

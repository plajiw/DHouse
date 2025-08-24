namespace DHouse.Core.DTOs
{
    public class ImovelListDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Bairro { get; set; }
        public decimal? ValorVenda { get; set; }
        public decimal? ValorAluguel { get; set; }
        public int? Quartos { get; set; }
        public int? VagasGaragemTotal { get; set; }
        public string? ImagemPrincipalUrl { get; set; }
    }
}

namespace Application.DTOs
{

    public class UpdateImovelDto
    {
        public string? TituloAnuncio { get; set; }
        public string? Descricao { get; set; }
        public StatusImovel { get; set; }
        public TipoDeImovel? Tipo { get; set; }
        public Endereco? Endereco { get; set; }
        public DetalhesFinanceiros? DetalhesFinanceiros { get; set; }
        public DetalhesDoImovel? DetalhesDoImovel { get; set; }
        public Infraestrutura? Infraestrutura { get; set; }
        public List<Midia>? Midias { get; set; }
    }
}

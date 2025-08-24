using DHouse.Core.Enums;
using DHouse.Core.ValueObjects;

namespace DHouse.Core.DTOs
{
    public class UpdateImovelDto
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public StatusImovel? Status { get; set; }
        public TipoDeImovel? Tipo { get; set; }
        public Endereco? Endereco { get; set; }
        public DetalhesFinanceiros? DetalhesFinanceiros { get; set; }
        public DetalhesDoImovel? DetalhesDoImovel { get; set; }
        public DetalhesDaInfraestrutura? DetalhesDaInfraestrutura { get; set; }
    }
}
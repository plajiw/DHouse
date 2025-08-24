using DHouse.Core.DTOs;
using DHouse.Core.Enums;
using DHouse.Core.ValueObjects;

namespace DHouse.Core.Entities
{
    public partial class Imovel
    {
        public string? Id { get; private set; }
        public Guid IdDoResponsavel { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public StatusImovel Status { get; private set; }
        public TipoDeImovel Tipo { get; private set; }
        public DateTime DataDeCadastro { get; private set; }
        public DateTime? DataDeAtualizacao { get; private set; }
        public Endereco Endereco { get; private set; }
        public DetalhesFinanceiros DetalhesFinanceiros { get; private set; }
        public DetalhesDoImovel DetalhesDoImovel { get; private set; }
        public DetalhesDaInfraestrutura DetalhesDaInfraestrutura { get; private set; }

        private readonly List<Midia> _midias = new();
        public IReadOnlyList<Midia> Midias => _midias.AsReadOnly();

        protected Imovel() { }

        public Imovel(
            Guid idDoResponsavel,
            string titulo,
            string descricao,
            TipoDeImovel tipo,
            Endereco endereco,
            DetalhesFinanceiros detalhesFinanceiros,
            DetalhesDoImovel detalhesDoImovel,
            DetalhesDaInfraestrutura detalhesDaInfraestrutura)
        {
            IdDoResponsavel = idDoResponsavel;
            Titulo = titulo;
            Descricao = descricao;
            Tipo = tipo;
            Endereco = endereco;
            DetalhesFinanceiros = detalhesFinanceiros;
            DetalhesDoImovel = detalhesDoImovel;
            DetalhesDaInfraestrutura = detalhesDaInfraestrutura;
            Status = StatusImovel.Rascunho;
            DataDeCadastro = DateTime.UtcNow;
        }


        public void AtualizarImovel(UpdateImovelDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (dto.Titulo != null)
                Titulo = dto.Titulo;

            if (dto.Descricao != null)
                Descricao = dto.Descricao;

            if (dto.Status.HasValue)
                Status = dto.Status.Value;

            if (dto.Tipo.HasValue)
                Tipo = dto.Tipo.Value;

            if (dto.Endereco != null)
                Endereco = dto.Endereco;

            if (dto.DetalhesFinanceiros != null)
                DetalhesFinanceiros = dto.DetalhesFinanceiros;

            if (dto.DetalhesDoImovel != null)
                DetalhesDoImovel = dto.DetalhesDoImovel;

            if (dto.DetalhesDaInfraestrutura != null)
                DetalhesDaInfraestrutura = dto.DetalhesDaInfraestrutura;

            MarcarComoAtualizado();
        }

        private void MarcarComoAtualizado()
        {
            DataDeAtualizacao = DateTime.UtcNow;
        }
    }
}
using DHouse.Core.Enums;
using DHouse.Core.ValueObjects;

namespace DHouse.Core.Entities
{
    public partial class Imovel
    {
        public string? Id { get; private set; }
        public Guid IdDoResponsavel { get; private set; }
        public string TituloAnuncio { get; private set; }
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
            string tituloAnuncio,
            string descricao,
            TipoDeImovel tipo,
            Endereco endereco,
            DetalhesFinanceiros detalhesFinanceiros,
            DetalhesDoImovel detalhesDoImovel,
            DetalhesDaInfraestrutura detalhesDaInfraestrutura)
        {
            IdDoResponsavel = idDoResponsavel;
            TituloAnuncio = tituloAnuncio;
            Descricao = descricao;
            Tipo = tipo;
            Endereco = endereco;
            DetalhesFinanceiros = detalhesFinanceiros;
            DetalhesDoImovel = detalhesDoImovel;
            DetalhesDaInfraestrutura = detalhesDaInfraestrutura;
            Status = StatusImovel.Rascunho;
            DataDeCadastro = DateTime.UtcNow;
        }

        public void AtualizarDadosPrincipais(string novoTitulo, string novaDescricao)
        {
            TituloAnuncio = novoTitulo;
            Descricao = novaDescricao;
            MarcarComoAtualizado();
        }

        public void AtualizarFinanceiro(DetalhesFinanceiros novosDetalhes)
        {
            DetalhesFinanceiros = novosDetalhes;
            MarcarComoAtualizado();
        }

        private void MarcarComoAtualizado()
        {
            DataDeAtualizacao = DateTime.UtcNow;
        }
    }
}
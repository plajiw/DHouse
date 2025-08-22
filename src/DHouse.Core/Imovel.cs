namespace DHouse.Core;

public class Imovel
{
    public Imovel()
    {
        DataCriacao = DateTime.Now;
        DataAtualizacao = DateTime.Now;
        Caracteristicas = new List<CaracteristicaImovel>();
    }

    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public TipoImovel TipoImovel { get; set; }
    public StatusImovel StatusImovel { get; set; } = StatusImovel.Rascunho;
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public Endereco Endereco { get; set; } = new Endereco();
    public List<CaracteristicaImovel> Caracteristicas { get; set; }
}
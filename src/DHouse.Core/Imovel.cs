namespace DHouse.Core;

public class Imovel
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal Preco {  get; set; }
    public TipoImovel TipoImovel { get; set; }
    public StatusImovel StatusImovel { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public Endereco Endereco { get; set; }
    public List<CaracteristicaImovel> Caracteristicas { get; set; } = new List<CaracteristicaImovel>();
}

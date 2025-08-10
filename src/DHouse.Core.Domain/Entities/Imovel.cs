using DHouse.Core.Domain.Common;
using DHouse.Core.Domain.Enums;
using DHouse.Core.Domain.ValueObjects;

namespace DHouse.Core.Domain.Entities;

public class Imovel : Entity
{
    public string Codigo { get; private set; } = default!;        // código interno/externo
    public string Titulo { get; private set; } = default!;
    public string? Descricao { get; private set; }

    public Endereco Endereco { get; private set; } = new();
    public decimal Preco { get; private set; }
    public StatusImovel Status { get; private set; } = StatusImovel.Ativo;

    public HashSet<string> Tags { get; private set; } = new();    // “garden”, “varanda”, etc.
    public string? CorretorId { get; private set; }               // referência (proprietário/listagem)

    public List<Imagem> Imagens { get; private set; } = new();
    public Imovel DefinirCapa(string imagemId)
    {
        Imagens = Imagens.Select(i => i with { IsCover = (i.Id == imagemId) }).ToList();
        return this;
    }
    public Imovel ReordenarImagens(IEnumerable<(string id, int ordem)> ordens)
    {
        var d = ordens.ToDictionary(x => x.id, x => x.ordem);
        Imagens = Imagens.Select(i => d.TryGetValue(i.Id, out var o) ? i with { Ordem = o } : i)
                         .OrderBy(i => i.Ordem).ToList();
        return this;
    }

    // Raven exige ctor público/sem parâmetros
    public Imovel() { }

    private Imovel(string codigo, string titulo, decimal preco, Endereco endereco)
    {
        SetCodigo(codigo);
        SetTitulo(titulo);
        SetPreco(preco);
        SetEndereco(endereco);
    }

    public static Imovel Create(string codigo, string titulo, decimal preco, Endereco endereco)
        => new(codigo, titulo, preco, endereco);

    public Imovel SetCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new ArgumentException("Código é obrigatório.");
        Codigo = codigo.Trim();
        return this;
    }

    public Imovel SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo)) throw new ArgumentException("Título é obrigatório.");
        Titulo = titulo.Trim();
        return this;
    }

    public Imovel SetDescricao(string? descricao)
    { Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim(); return this; }

    public Imovel SetEndereco(Endereco endereco)
    { Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco)); return this; }

    public Imovel SetPreco(decimal preco)
    {
        if (preco < 0) throw new ArgumentOutOfRangeException(nameof(preco), "Preço não pode ser negativo.");
        Preco = decimal.Round(preco, 2);
        return this;
    }

    public Imovel DefinirStatus(StatusImovel status) { Status = status; return this; }

    public Imovel AtribuirCorretor(string? corretorId) { CorretorId = string.IsNullOrWhiteSpace(corretorId) ? null : corretorId; return this; }

    public Imovel AddTag(string tag) { if (!string.IsNullOrWhiteSpace(tag)) Tags.Add(tag.Trim().ToLowerInvariant()); return this; }

    public Imovel RemoveTag(string tag) { if (!string.IsNullOrWhiteSpace(tag)) Tags.Remove(tag.Trim().ToLowerInvariant()); return this; }
}

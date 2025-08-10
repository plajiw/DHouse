using System.Linq;
using DHouse.Core.Application.DTOs.Imoveis;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.ValueObjects;

namespace DHouse.Core.Application.Mappings;

public static class ImovelMappings
{
    public static Imovel ToEntity(this CriarImovelDto dto)
    {
        var endereco = new Endereco(dto.Endereco.Cidade, dto.Endereco.Estado, dto.Endereco.Bairro,
                                    dto.Endereco.Logradouro, dto.Endereco.Numero, dto.Endereco.Cep);
        var entity = Imovel.Create(dto.Codigo, dto.Titulo, dto.Preco, endereco)
                           .SetDescricao(dto.Descricao);
        if (dto.Tags != null)
            foreach (var t in dto.Tags) entity.AddTag(t);
        return entity;
    }

    public static void Apply(this Imovel entity, AtualizarImovelDto dto)
    {
        if (dto.Titulo != null) entity.SetTitulo(dto.Titulo);
        if (dto.Descricao != null) entity.SetDescricao(dto.Descricao);
        if (dto.Preco.HasValue) entity.SetPreco(dto.Preco.Value);
        if (dto.Endereco != null)
        {
            var e = dto.Endereco;
            entity.SetEndereco(new Endereco(e.Cidade, e.Estado, e.Bairro, e.Logradouro, e.Numero, e.Cep));
        }
        if (dto.Tags != null)
        {
            entity.Tags.Clear();
            foreach (var t in dto.Tags) entity.AddTag(t);
        }
        entity.MarcarAtualizado();
    }

    public static ImovelDto ToDto(this Imovel i) =>
        new(i.Id, i.Codigo, i.Titulo, i.Descricao, i.Preco, i.Status.ToString(),
            i.Endereco.Cidade, i.Endereco.Bairro, i.Endereco.Estado, i.Tags.ToArray(), i.CorretorId);
}

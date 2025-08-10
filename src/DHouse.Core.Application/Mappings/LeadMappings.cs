using DHouse.Core.Application.DTOs.Leads;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.Enums;

namespace DHouse.Core.Application.Mappings;

public static class LeadMappings
{
    public static Lead ToEntity(this CriarLeadDto dto)
        => Lead.Create(dto.Nome, dto.Origem, dto.ImovelId)
               .SetTelefone(dto.Telefone)
               .SetEmail(dto.Email);

    public static void Apply(this Lead entity, AtualizarLeadDto dto)
    {
        if (dto.Nome != null) entity.SetNome(dto.Nome);
        if (dto.Telefone != null) entity.SetTelefone(dto.Telefone);
        if (dto.Email != null) entity.SetEmail(dto.Email);
        if (dto.Observacoes != null) entity.SetObservacoes(dto.Observacoes);
        if (dto.Status.HasValue) entity.MudarStatus(dto.Status.Value);
        entity.MarcarAtualizado();
    }

    public static LeadDto ToDto(this Lead l) =>
        new(l.Id, l.Nome, l.Telefone, l.Email, l.Origem.ToString(), l.Status.ToString(),
            l.ImovelId, l.Observacoes,
            l.PrimeiroContatoEm?.ToString("O"),
            l.UltimoContatoEm?.ToString("O"));
}

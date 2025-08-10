using DHouse.Core.Domain.Enums;

namespace DHouse.Core.Application.DTOs.Leads;

public record CriarLeadDto(
    string Nome,
    string? Telefone,
    string? Email,
    OrigemLead Origem = OrigemLead.Site,
    string? ImovelId = null
);

public record AtualizarLeadDto(
    string? Nome,
    string? Telefone,
    string? Email,
    string? Observacoes,
    StatusLead? Status
);

public record LeadDto(
    string Id,
    string Nome,
    string? Telefone,
    string? Email,
    string Origem,
    string Status,
    string? ImovelId,
    string? Observacoes,
    string? PrimeiroContatoEm,
    string? UltimoContatoEm
);

using DHouse.Core.Domain.Common;
using DHouse.Core.Domain.Enums;

namespace DHouse.Core.Domain.Entities;

public class Lead : Entity
{
    public string Nome { get; private set; } = default!;
    public string? Telefone { get; private set; }
    public string? Email { get; private set; }

    public OrigemLead Origem { get; private set; } = OrigemLead.Site;
    public StatusLead Status { get; private set; } = StatusLead.Novo;

    public string? ImovelId { get; private set; }   // lead pode estar associado a um imóvel
    public string? Observacoes { get; private set; }

    public DateTimeOffset? PrimeiroContatoEm { get; private set; }
    public DateTimeOffset? UltimoContatoEm { get; private set; }

    public Lead() { }

    private Lead(string nome, OrigemLead origem, string? imovelId)
    {
        SetNome(nome);
        Origem = origem;
        ImovelId = string.IsNullOrWhiteSpace(imovelId) ? null : imovelId;
    }

    public static Lead Create(string nome, OrigemLead origem = OrigemLead.Site, string? imovelId = null)
        => new(nome, origem, imovelId);

    public Lead SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.");
        Nome = nome.Trim();
        return this;
    }

    public Lead SetTelefone(string? telefone) { Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim(); return this; }
    public Lead SetEmail(string? email) { Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant(); return this; }
    public Lead SetObservacoes(string? obs) { Observacoes = string.IsNullOrWhiteSpace(obs) ? null : obs.Trim(); return this; }

    public Lead VincularImovel(string? imovelId) { ImovelId = string.IsNullOrWhiteSpace(imovelId) ? null : imovelId; return this; }

    public Lead MudarStatus(StatusLead status)
    { Status = status; if (status >= StatusLead.EmContato) MarcarContato(); return this; }

    public Lead MarcarContato()
    {
        var agora = DateTimeOffset.UtcNow;
        PrimeiroContatoEm ??= agora;
        UltimoContatoEm = agora;
        return this;
    }
}

using DHouse.Core.Domain.Common;

namespace DHouse.Core.Domain.Entities;

public class Corretor : Entity
{
    public string Nome { get; private set; } = default!;
    public string? CpfCnpj { get; private set; }
    public string? Email { get; private set; }
    public string? Telefone { get; private set; }

    public bool Ativo { get; private set; } = true;

    public Corretor() { }

    private Corretor(string nome) => SetNome(nome);

    public static Corretor Create(string nome) => new(nome);

    public Corretor SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.");
        Nome = nome.Trim();
        return this;
    }

    public Corretor SetCpfCnpj(string? cpfCnpj) { CpfCnpj = string.IsNullOrWhiteSpace(cpfCnpj) ? null : cpfCnpj.Trim(); return this; }
    public Corretor SetEmail(string? email) { Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant(); return this; }
    public Corretor SetTelefone(string? tel) { Telefone = string.IsNullOrWhiteSpace(tel) ? null : tel.Trim(); return this; }

    public Corretor Ativar() { Ativo = true; return this; }
    public Corretor Desativar() { Ativo = false; return this; }
}

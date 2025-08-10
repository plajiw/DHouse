namespace DHouse.Core.Domain.Entities;
public class Usuario
{
    public string Id { get; set; } = default!;
    public string Email { get; private set; } = default!;
    public string Nome { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string[] Roles { get; private set; } = Array.Empty<string>();
    public bool Ativo { get; private set; } = true;

    public static Usuario Create(string email, string nome, string passwordHash, IEnumerable<string> roles) =>
        new Usuario { Email = email.Trim().ToLowerInvariant(), Nome = nome, PasswordHash = passwordHash, Roles = roles?.ToArray() ?? Array.Empty<string>() };
}

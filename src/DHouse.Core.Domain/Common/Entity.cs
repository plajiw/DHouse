namespace DHouse.Core.Domain.Common;

public abstract class Entity
{
    public string Id { get; protected set; } = default!;

    public DateTimeOffset CriadoEm { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? AtualizadoEm { get; protected set; }

    public void MarcarAtualizado() => AtualizadoEm = DateTimeOffset.UtcNow;
}

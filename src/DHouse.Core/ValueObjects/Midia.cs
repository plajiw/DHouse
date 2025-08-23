using DHouse.Core.Enums;

namespace DHouse.Core.ValueObjects
{
    public record Midia(
        Guid Id,
        string Url,
        TipoMidia Tipo,
        int Ordem = 0,
        string? Legenda = null,
        bool IsPrincipal = false
    );
}

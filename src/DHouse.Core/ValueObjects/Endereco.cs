using DHouse.Core.Enums;

namespace DHouse.Core.ValueObjects
{
    public record Endereco(
        string Logradouro,
        string Numero,
        string Bairro,
        string Cidade,
        Estado UF,
        string CEP,
        string? Complemento = null,
        decimal? Latitude = null,
        decimal? Longitude = null,
        string? LinkGoogleMaps = null
    )
    { }
}
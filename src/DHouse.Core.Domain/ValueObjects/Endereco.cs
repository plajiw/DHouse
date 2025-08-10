namespace DHouse.Core.Domain.ValueObjects;

// VO com igualdade por valor (record) + ctor sem parâmetros para serialização
public record Endereco(
    string Cidade,
    string Estado,
    string Bairro,
    string Logradouro,
    string Numero,
    string Cep)
{
    // Ctor parameterless para (de)serializadores e Raven
    public Endereco() : this("", "", "", "", "", "") { }

    public static Endereco Create(
        string cidade, string estado, string bairro,
        string logradouro, string numero, string cep)
    {
        // Validações mínimas (ex.: obrigatórios); ajuste conforme regra
        if (string.IsNullOrWhiteSpace(cidade)) throw new ArgumentException("Cidade obrigatória.");
        if (string.IsNullOrWhiteSpace(estado)) throw new ArgumentException("Estado obrigatório.");
        if (string.IsNullOrWhiteSpace(cep)) throw new ArgumentException("CEP obrigatório.");
        return new Endereco(cidade.Trim(), estado.Trim(), bairro?.Trim() ?? "",
                            logradouro?.Trim() ?? "", numero?.Trim() ?? "", cep.Trim());
    }

    public override string ToString() =>
        $"{Logradouro}, {Numero} - {Bairro}, {Cidade}-{Estado}, {Cep}";
}

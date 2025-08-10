namespace DHouse.Core.Application.DTOs.Imoveis;

public record EnderecoDto(string Cidade, string Estado, string Bairro, string Logradouro, string Numero, string Cep);

public record CriarImovelDto(
    string Codigo,
    string Titulo,
    string? Descricao,
    decimal Preco,
    EnderecoDto Endereco,
    IEnumerable<string>? Tags
);

public record AtualizarImovelDto(
    string? Titulo,
    string? Descricao,
    decimal? Preco,
    EnderecoDto? Endereco,
    IEnumerable<string>? Tags
);

public record ImovelDto(
    string Id,
    string Codigo,
    string Titulo,
    string? Descricao,
    decimal Preco,
    string Status,
    string Cidade,
    string Bairro,
    string Estado,
    IEnumerable<string> Tags,
    string? CorretorId
);

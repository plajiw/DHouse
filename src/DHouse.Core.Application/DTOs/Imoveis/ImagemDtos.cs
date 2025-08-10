namespace DHouse.Core.Application.DTOs.Imoveis;

public record ImagemDto(string Id, string Url, string ThumbUrl, string? Alt, int Ordem, bool IsCover);
public record DefinirCapaDto(string ImagemId);
public record ReordenarImagensDto(IReadOnlyList<string> ImagensOrdenadas); // na ordem final

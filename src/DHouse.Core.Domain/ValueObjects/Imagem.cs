namespace DHouse.Core.Domain.ValueObjects;

public record Imagem(
    string Id,            // ex.: "img_2a7c..."
    string Url,           // /uploads/imoveis/{imovelId}/md_{guid}.webp
    string ThumbUrl,      // /uploads/imoveis/{imovelId}/thumb_{guid}.webp
    string? Alt,
    int Ordem,
    bool IsCover,
    int Width,
    int Height,
    long Bytes
)
{
    public static Imagem Create(string id, string url, string thumbUrl, string? alt,
                                int ordem, bool isCover, int width, int height, long bytes)
        => new(id, url, thumbUrl, alt, ordem, isCover, width, height, bytes);
}

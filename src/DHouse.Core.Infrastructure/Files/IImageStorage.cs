namespace DHouse.Core.Infrastructure.Files;

public interface IImageStorage
{
    Task<(string Url, string ThumbUrl, int Width, int Height, long Bytes, string StoredId)>
        SaveAsync(string imovelId, Stream file, string contentType, CancellationToken ct);

    Task DeleteAsync(string imovelId, string storedId, CancellationToken ct);
}

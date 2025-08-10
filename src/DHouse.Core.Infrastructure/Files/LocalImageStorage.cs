using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace DHouse.Core.Infrastructure.Files
{
    using ImageSharpImage = SixLabors.ImageSharp.Image;

    public class LocalImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;
        public LocalImageStorage(IWebHostEnvironment env) => _env = env;

        public async Task<(string Url, string ThumbUrl, int Width, int Height, long Bytes, string StoredId)>
            SaveAsync(string imovelId, Stream file, string contentType, CancellationToken ct)
        {
            // valida tipo
            if (string.IsNullOrWhiteSpace(contentType) || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Arquivo não é imagem.");

            var safeImovelId = Sanitize(imovelId);
            var guid = Guid.NewGuid().ToString("N");
            var webRoot = _env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
            var basePath = Path.Combine(webRoot, "uploads", "imoveis", safeImovelId);
            Directory.CreateDirectory(basePath);

            // carrega imagem (também valida se de fato é imagem)
            using var image = await ImageSharpImage.LoadAsync(file, ct);

            var mdPath = Path.Combine(basePath, $"md_{guid}.webp");
            var thumbPath = Path.Combine(basePath, $"thumb_{guid}.webp");

            var encoder = new WebpEncoder { Quality = 85 };

            // md (1280px máx)
            using (var md = image.Clone(x => x.Resize(new ResizeOptions
            { Mode = ResizeMode.Max, Size = new Size(1280, 1280) })))
            {
                await md.SaveAsync(mdPath, encoder, ct);
            }

            // thumb 16:9 (320x180 crop)
            using (var thumb = image.Clone(x => x.Resize(new ResizeOptions
            { Mode = ResizeMode.Crop, Size = new Size(320, 180) })))
            {
                await thumb.SaveAsync(thumbPath, encoder, ct);
            }

            var fi = new FileInfo(mdPath);
            var url = $"/uploads/imoveis/{safeImovelId}/md_{guid}.webp";
            var thumbUrl = $"/uploads/imoveis/{safeImovelId}/thumb_{guid}.webp";

            return (url, thumbUrl, image.Width, image.Height, fi.Length, guid);
        }

        public Task DeleteAsync(string imovelId, string storedId, CancellationToken ct)
        {
            var safeImovelId = Sanitize(imovelId);
            var webRoot = _env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot");
            var basePath = Path.Combine(webRoot, "uploads", "imoveis", safeImovelId);

            var files = new[] { $"md_{storedId}.webp", $"thumb_{storedId}.webp" }
                .Select(f => Path.Combine(basePath, f));

            foreach (var f in files)
                if (File.Exists(f)) File.Delete(f);

            return Task.CompletedTask;
        }

        private static string Sanitize(string input)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var cleaned = new string(input.Where(c => !invalid.Contains(c)).ToArray());
            return string.IsNullOrWhiteSpace(cleaned) ? "unknown" : cleaned;
        }
    }
}

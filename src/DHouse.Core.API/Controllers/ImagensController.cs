using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Application.DTOs.Imoveis;
using DHouse.Core.Infrastructure.Files;

[ApiController]
[Route("api/imoveis/{id}/imagens")]
public class ImagensController : ControllerBase
{
    private readonly IAsyncDocumentSession _session;
    private readonly IImageStorage _storage;
    public ImagensController(IAsyncDocumentSession s, IImageStorage st) { _session = s; _storage = st; }

    // Upload múltiplo (multipart/form-data)
    [HttpPost]
    [RequestSizeLimit(20_000_000)] // 20 MB por request
    public async Task<IActionResult> Upload(string id, [FromForm] IFormFileCollection files, [FromForm] string? alt)
    {
        var imovel = await _session.LoadAsync<Imovel>(id);
        if (imovel is null) return NotFound();

        int ordemBase = imovel.Imagens.Count == 0 ? 1 : imovel.Imagens.Max(i => i.Ordem) + 1;

        foreach (var f in files)
        {
            if (f.Length == 0) continue;

            await using var stream = f.OpenReadStream();
            var (url, thumbUrl, w, h, bytes, storedId) =
                await _storage.SaveAsync(imovel.Id, stream, f.ContentType, HttpContext.RequestAborted);

            var imgId = $"img_{storedId}";
            var img = DHouse.Core.Domain.ValueObjects.Imagem.Create(imgId, url, thumbUrl, alt, ordemBase++, false, w, h, bytes);
            imovel.Imagens.Add(img);
        }

        await _session.SaveChangesAsync();
        return Ok(imovel.Imagens.Select(i => new ImagemDto(i.Id, i.Url, i.ThumbUrl, i.Alt, i.Ordem, i.IsCover)));
    }

    [HttpPut("capa")]
    public async Task<IActionResult> DefinirCapa(string id, [FromBody] DefinirCapaDto dto)
    {
        var imovel = await _session.LoadAsync<Imovel>(id);
        if (imovel is null) return NotFound();
        imovel.DefinirCapa(dto.ImagemId);
        await _session.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("ordem")]
    public async Task<IActionResult> Reordenar(string id, [FromBody] ReordenarImagensDto dto)
    {
        var imovel = await _session.LoadAsync<Imovel>(id);
        if (imovel is null) return NotFound();
        var ordens = dto.ImagensOrdenadas.Select((imgId, idx) => (imgId, ordem: idx + 1));
        imovel.ReordenarImagens(ordens);
        await _session.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{imagemId}")]
    public async Task<IActionResult> Remover(string id, string imagemId)
    {
        var imovel = await _session.LoadAsync<Imovel>(id);
        if (imovel is null) return NotFound();

        var img = imovel.Imagens.FirstOrDefault(x => x.Id == imagemId);
        if (img is null) return NotFound();

        var storedId = img.Id.Replace("img_", "");
        await _storage.DeleteAsync(imovel.Id, storedId, HttpContext.RequestAborted);

        imovel.Imagens.Remove(img);
        await _session.SaveChangesAsync();
        return NoContent();
    }
}

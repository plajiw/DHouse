using DHouse.Core.Application.DTOs.Imoveis;
using DHouse.Core.Application.Mappings;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.Enums;
using DHouse.Core.Infrastructure.Raven.Indexes;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace DHouse.Core.API.Controllers;

[ApiController]
[Route("api/imoveis")]
public class ImoveisController : ControllerBase
{
    private readonly IAsyncDocumentSession _session;
    public ImoveisController(IAsyncDocumentSession session) => _session = session;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarImovelDto dto)
    {
        var entity = dto.ToEntity();
        await _session.StoreAsync(entity);
        await _session.SaveChangesAsync();
        return CreatedAtAction(nameof(ObterPorId), new { id = entity.Id }, entity.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImovelDto>> ObterPorId(string id)
    {
        var entity = await _session.LoadAsync<Imovel>(id);
        return entity is null ? NotFound() : Ok(entity.ToDto());
    }

    public record BuscarImoveisQuery(string? Cidade, string? Bairro, StatusImovel? Status, decimal? MinPreco, decimal? MaxPreco, string? Tag);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImovelDto>>> Buscar([FromQuery] BuscarImoveisQuery q)
    {
        IRavenQueryable<Imovel> query = _session.Query<Imovel, Imoveis_PorFiltro>();

        if (!string.IsNullOrWhiteSpace(q.Cidade))
            query = query.Where(i => i.Endereco.Cidade == q.Cidade);
        if (!string.IsNullOrWhiteSpace(q.Bairro))
            query = query.Where(i => i.Endereco.Bairro == q.Bairro);
        if (q.Status.HasValue)
            query = query.Where(i => i.Status == q.Status.Value);
        if (q.MinPreco.HasValue)
            query = query.Where(i => i.Preco >= q.MinPreco.Value);
        if (q.MaxPreco.HasValue)
            query = query.Where(i => i.Preco <= q.MaxPreco.Value);
        if (!string.IsNullOrWhiteSpace(q.Tag))
            query = query.Where(i => i.Tags.Contains(q.Tag.ToLower()));

        var results = await query.Take(100).ToListAsync();
        return Ok(results.Select(r => r.ToDto()));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(string id, [FromBody] AtualizarImovelDto dto)
    {
        var entity = await _session.LoadAsync<Imovel>(id);
        if (entity is null) return NotFound();

        entity.Apply(dto);
        await _session.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(string id)
    {
        var entity = await _session.LoadAsync<Imovel>(id);
        if (entity is null) return NotFound();
        _session.Delete(entity);
        await _session.SaveChangesAsync();
        return NoContent();
    }
}

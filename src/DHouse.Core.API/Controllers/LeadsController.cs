using DHouse.Core.Application.DTOs.Leads;
using DHouse.Core.Application.Mappings;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace DHouse.Core.API.Controllers;

[ApiController]
[Route("api/leads")]
public class LeadsController : ControllerBase
{
    private readonly IAsyncDocumentSession _session;
    public LeadsController(IAsyncDocumentSession session) => _session = session;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarLeadDto dto)
    {
        var entity = dto.ToEntity();
        await _session.StoreAsync(entity);
        await _session.SaveChangesAsync();
        return CreatedAtAction(nameof(ObterPorId), new { id = entity.Id }, entity.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeadDto>> ObterPorId(string id)
    {
        var entity = await _session.LoadAsync<Lead>(id);
        return entity is null ? NotFound() : Ok(entity.ToDto());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeadDto>>> Listar([FromQuery] StatusLead? status)
    {
        var query = _session.Query<Lead>();
        if (status.HasValue) query = query.Where(l => l.Status == status.Value);
        var list = await query.Take(100).ToListAsync();
        return Ok(list.Select(x => x.ToDto()));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(string id, [FromBody] AtualizarLeadDto dto)
    {
        var entity = await _session.LoadAsync<Lead>(id);
        if (entity is null) return NotFound();

        entity.Apply(dto);
        await _session.SaveChangesAsync();
        return NoContent();
    }
}

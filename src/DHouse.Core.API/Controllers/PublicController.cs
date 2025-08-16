using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using DHouse.Core.Domain.Enums;
using LeadEntity = DHouse.Core.Domain.Entities.Lead; // <<< alias

namespace DHouse.Core.API.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly IAsyncDocumentSession _session;
    public PublicController(IAsyncDocumentSession s) => _session = s;

    public record LeadPublicDto(string Nome, string? Telefone, string? Email, bool AceiteLgpd);

    [HttpPost("lead")]
    public async Task<IActionResult> Lead([FromBody] LeadPublicDto dto)
    {
        if (!dto.AceiteLgpd || string.IsNullOrWhiteSpace(dto.Nome))
            return BadRequest("Consentimento LGPD e nome são obrigatórios.");

        var lead = LeadEntity.Create(dto.Nome, OrigemLead.Site, null) // <<< usa alias
                             .SetTelefone(dto.Telefone)
                             .SetEmail(dto.Email)
                             .SetObservacoes("Lead público (API). Consentimento LGPD: OK.");

        await _session.StoreAsync(lead);
        await _session.SaveChangesAsync();

        return Ok(new { id = lead.Id });
    }
}

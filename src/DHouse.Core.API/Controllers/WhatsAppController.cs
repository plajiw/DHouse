using Microsoft.AspNetCore.Mvc;
using DHouse.Core.Application.DTOs.WhatsApp;
using DHouse.Core.Infrastructure.WhatsApp;

namespace DHouse.Core.API.Controllers;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppClient _wa;

    public WhatsAppController(IWhatsAppClient wa) => _wa = wa;

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendWhatsAppTextDto dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.To) || string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest("Destinatário e mensagem são obrigatórios.");

        await _wa.SendTextAsync(dto.To, dto.Message, ct);
        return Ok(new { sent = true });
    }
}

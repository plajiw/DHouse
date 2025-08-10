using DHouse.Core.Application.DTOs.WhatsApp;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.Enums;
using DHouse.Core.Infrastructure.Raven.Indexes;
using DHouse.Core.Infrastructure.WhatsApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace DHouse.Core.API.Controllers;

[ApiController]
[Route("webhooks/whatsapp")]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly IOptions<WhatsAppOptions> _opt;
    private readonly IAsyncDocumentSession _session;

    public WhatsAppWebhookController(IOptions<WhatsAppOptions> opt, IAsyncDocumentSession session)
    {
        _opt = opt;
        _session = session;
    }

    // Verificação do webhook (GET)
    // Meta chama com ?hub.mode=subscribe&hub.verify_token=...&hub.challenge=...
    [HttpGet]
    public IActionResult Verify([FromQuery(Name = "hub.mode")] string? mode,
                                [FromQuery(Name = "hub.verify_token")] string? verifyToken,
                                [FromQuery(Name = "hub.challenge")] string? challenge)
    {
        if (mode == "subscribe" && verifyToken == _opt.Value.VerifyToken)
            return Content(challenge ?? string.Empty);

        return Unauthorized();
    }

    // Recebimento de mensagens (POST)
    [HttpPost]
    public async Task<ActionResult<WhatsAppHookAckDto>> Receive([FromBody] dynamic body, CancellationToken ct)
    {
        // Parsing minimalista para v19.0 — só texto
        // Estrutura: entry[0].changes[0].value.messages[0]
        try
        {
            var entries = body?.entry;
            if (entries == null) return Ok(new WhatsAppHookAckDto());

            foreach (var entry in entries)
            {
                var changes = entry?.changes;
                if (changes == null) continue;

                foreach (var change in changes)
                {
                    var value = change?.value;
                    var messages = value?.messages;
                    if (messages == null) continue;

                    foreach (var msg in messages)
                    {
                        var from = (string?)msg?.from;           // telefone do cliente (E164)
                        var textBody = (string?)msg?.text?.body; // texto
                        if (string.IsNullOrWhiteSpace(from)) continue;

                        // Normalização simples do telefone (e.g., remover espaços)
                        var phone = from.Trim();

                        // Tenta localizar Lead por telefone
                        var lead = await _session.Query<Lead, Leads_PorTelefone>()
                                                 .Where(l => l.Telefone == phone)
                                                 .FirstOrDefaultAsync(ct);

                        if (lead is null)
                        {
                            lead = Lead.Create(nome: $"Contato {phone}", origem: OrigemLead.Whatsapp, imovelId: null)
                                       .SetTelefone(phone)
                                       .SetObservacoes(textBody);
                            await _session.StoreAsync(lead, ct);
                        }
                        else
                        {
                            // Atualiza status e contato
                            lead.SetObservacoes(textBody);
                            lead.MudarStatus(StatusLead.EmContato);
                            lead.MarcarContato();
                        }
                    }
                }
            }

            await _session.SaveChangesAsync(ct);
            return Ok(new WhatsAppHookAckDto());
        }
        catch
        {
            // MVP: não derruba, apenas ACK
            return Ok(new WhatsAppHookAckDto());
        }
    }
}

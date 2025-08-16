using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.Enums;
using DHouse.Core.Infrastructure.Raven.Indexes;

namespace DHouse.Core.API.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "SomenteAdmin")]
public class LeadsController : Controller
{
    private readonly IAsyncDocumentSession _session;
    public LeadsController(IAsyncDocumentSession s) => _session = s;

    [HttpGet("/admin/leads")]
    public async Task<IActionResult> Index(StatusLead? status, int page = 1, int pageSize = 20)
    {
        var q = _session.Query<Lead, Leads_PorStatusData>();
        if (status.HasValue) q = q.Where(l => l.Status == status.Value);
        var list = await q.OrderByDescending(l => l.CriadoEm)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();
        if (Request.Headers["HX-Request"] == "true")
            return PartialView("_LeadsTable", list);
        return View(list);
    }

    [HttpGet("/admin/leads/novo")]
    public IActionResult Novo() => View(new Lead { });

    [ValidateAntiForgeryToken]
    [HttpPost("/admin/leads/novo")]
    public async Task<IActionResult> NovoPost(string nome, string? telefone, string? email, string? observacoes)
    {
        var lead = Lead.Create(nome, OrigemLead.Site, null)
                       .SetTelefone(telefone)
                       .SetEmail(email)
                       .SetObservacoes(observacoes);
        await _session.StoreAsync(lead);
        await _session.SaveChangesAsync();
        return Redirect("/admin/leads");
    }

    [HttpGet("/admin/leads/{id}")]
    public async Task<IActionResult> Editar(string id)
    {
        var l = await _session.LoadAsync<Lead>(id);
        if (l is null) return NotFound();
        return View(l);
    }

    [ValidateAntiForgeryToken]
    [HttpPost("/admin/leads/{id}/status")]
    public async Task<IActionResult> MudarStatus(string id, StatusLead status)
    {
        var l = await _session.LoadAsync<Lead>(id);
        if (l is null) return NotFound();
        l.MudarStatus(status);
        await _session.SaveChangesAsync();

        // Para HTMX: retorna apenas o “badge” atualizado
        if (Request.Headers["HX-Request"] == "true")
            return PartialView("_LeadBadge", l);
        return Redirect($"/admin/leads/{id}");
    }
}

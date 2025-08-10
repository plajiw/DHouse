using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace DHouse.Core.Infrastructure.WhatsApp;

public sealed class WhatsAppCloudClient : IWhatsAppClient
{
    private readonly HttpClient _http;
    private readonly WhatsAppOptions _opt;

    public WhatsAppCloudClient(HttpClient http, WhatsAppOptions opt)
    {
        _http = http;
        _opt = opt;
    }

    public async Task SendTextAsync(string toE164, string body, CancellationToken ct = default)
    {
        // Monta URL dinâmica com PhoneNumberId
        var url = $"https://graph.facebook.com/v19.0/{_opt.PhoneNumberId}/messages";
        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _opt.Token);

        var payload = new
        {
            messaging_product = "whatsapp",
            to = toE164,            // Ex.: "5562999990000"
            type = "text",
            text = new { body }
        };

        req.Content = JsonContent.Create(payload, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        using var res = await _http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode(); // Se falhar, lança exceção — simples no MVP
    }
}

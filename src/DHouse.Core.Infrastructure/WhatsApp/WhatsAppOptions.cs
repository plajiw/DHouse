namespace DHouse.Core.Infrastructure.WhatsApp;

public sealed class WhatsAppOptions
{
    public string PhoneNumberId { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string VerifyToken { get; set; } = default!;
}

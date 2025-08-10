namespace DHouse.Core.Application.DTOs.WhatsApp;

public record SendWhatsAppTextDto(string To, string Message);
public record WhatsAppHookAckDto(string Status = "ok");

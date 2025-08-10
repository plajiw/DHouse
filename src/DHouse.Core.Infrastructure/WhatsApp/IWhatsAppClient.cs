using System.Threading;
using System.Threading.Tasks;

namespace DHouse.Core.Infrastructure.WhatsApp;

public interface IWhatsAppClient
{
    Task SendTextAsync(string toE164, string body, CancellationToken ct = default);
}

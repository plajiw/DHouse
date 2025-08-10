using System.Linq;
using Raven.Client.Documents.Indexes;
using DHouse.Core.Domain.Entities;

namespace DHouse.Core.Infrastructure.Raven.Indexes;

public class Leads_PorTelefone : AbstractIndexCreationTask<Lead>
{
    public Leads_PorTelefone()
    {
        Map = leads => from l in leads
                       select new { l.Telefone };
        // Para busca exata; se quiser normalizar, faça no domínio.
    }
}

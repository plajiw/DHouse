using System.Linq;
using Raven.Client.Documents.Indexes;
using DHouse.Core.Domain.Entities;
using DHouse.Core.Domain.Enums;

namespace DHouse.Core.Infrastructure.Raven.Indexes;

public class Leads_PorStatusData : AbstractIndexCreationTask<Lead>
{
    public Leads_PorStatusData()
    {
        Map = leads => from l in leads
                       select new { l.Status, l.CriadoEm };
    }
}

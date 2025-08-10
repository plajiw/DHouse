using System.Linq;
using Raven.Client.Documents.Indexes;
using DHouse.Core.Domain.Entities;

namespace DHouse.Core.Infrastructure.Raven.Indexes;

public class Usuarios_PorEmail : AbstractIndexCreationTask<Usuario>
{
    public Usuarios_PorEmail()
    {
        Map = users => from u in users select new { u.Email };
    }
}

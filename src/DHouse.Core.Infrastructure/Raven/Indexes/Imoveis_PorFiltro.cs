using System.Linq;
using Raven.Client.Documents.Indexes;
using DHouse.Core.Domain.Entities;

namespace DHouse.Core.Infrastructure.Raven.Indexes;

public class Imoveis_PorFiltro : AbstractIndexCreationTask<Imovel>
{
    public Imoveis_PorFiltro()
    {
        Map = imoveis => from i in imoveis
                         select new
                         {
                             i.Codigo,
                             i.Preco,
                             i.Status,
                             i.Tags,
                             i.Endereco.Cidade,
                             i.Endereco.Bairro
                         };
    }
}

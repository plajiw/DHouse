using DHouse.Core.DTOs;
using DHouse.Core.Entities;

namespace DHouse.Core.Interfaces
{
    public interface IImovelRepository
    {
        Task<Imovel?> ObterImovelPorIdAsync(string id);
        Task AdicionarImovelAsync(Imovel imovel);
        Task AtualizarImovelAsync(Imovel imovel);
        Task RemoverImovelAsync(string id);
        Task<ResultadoPaginado<ListaImovelDto>> BuscaAsync(CriteriosDeBuscaDeImoveis criteria, int pageNumber, int pageSize);

    }
}

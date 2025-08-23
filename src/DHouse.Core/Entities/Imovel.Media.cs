using DHouse.Core.Enums;
using DHouse.Core.ValueObjects;

namespace DHouse.Core.Entities
{
    public partial class Imovel
    {
        public void AdicionarMidia(string url, TipoMidia tipo, string? legenda, bool isPrincipal = false)
        {
            if (_midias.Any(m => m.Url == url)) return;

            if (isPrincipal)
            {
                _midias.ForEach(m => _midias[_midias.IndexOf(m)] = m with { IsPrincipal = false });
            }

            var novaMidia = new Midia(
                Id: Guid.NewGuid(),
                Url: url,
                Tipo: tipo,
                Ordem: _midias.Count + 1,
                Legenda: legenda,
                IsPrincipal: isPrincipal
            );

            _midias.Add(novaMidia);
            MarcarComoAtualizado();
        }

        public void RemoverMidia(Guid midiaId)
        {
            var midiaParaRemover = _midias.FirstOrDefault(m => m.Id == midiaId);
            if (midiaParaRemover != null)
            {
                _midias.Remove(midiaParaRemover);
                MarcarComoAtualizado();
            }
        }

        public void DefinirMidiaPrincipal(Guid midiaId)
        {
            if (!_midias.Any(m => m.Id == midiaId)) return;

            _midias.ForEach(m => _midias[_midias.IndexOf(m)] = m with { IsPrincipal = m.Id == midiaId });

            MarcarComoAtualizado();
        }

        public void ReordenarMidias(List<Guid> novaOrdemDeIds)
        {
            var midiasOrdenadas = new List<Midia>();
            int ordem = 1;

            foreach (var id in novaOrdemDeIds)
            {
                var midia = _midias.FirstOrDefault(m => m.Id == id);
                if (midia != null)
                {
                    midiasOrdenadas.Add(midia with { Ordem = ordem++ });
                }
            }

            var midiasRestantes = _midias.Where(m => !novaOrdemDeIds.Contains(m.Id));
            foreach (var midia in midiasRestantes)
            {
                midiasOrdenadas.Add(midia with { Ordem = ordem++ });
            }

            _midias.Clear();
            _midias.AddRange(midiasOrdenadas);
            MarcarComoAtualizado();
        }
    }
}
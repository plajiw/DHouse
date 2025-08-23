using DHouse.Core.Enums;

namespace DHouse.Core.Entities
{
    public partial class Imovel
    {
        public void MarcarComoDisponivel()
        {
            if (Status == StatusImovel.Rascunho || Status == StatusImovel.Inativo)
            {
                Status = StatusImovel.Disponivel;
                MarcarComoAtualizado();
            }
        }

        public void MarcarComoVendido()
        {
            if (Status == StatusImovel.Disponivel)
            {
                Status = StatusImovel.Vendido;
                MarcarComoAtualizado();
            }
        }

        public void MarcarComoAlugado()
        {
            if (Status == StatusImovel.Disponivel)
            {
                Status = StatusImovel.Alugado;
                MarcarComoAtualizado();
            }
        }

        public void MarcarComoInativo()
        {
            if (Status == StatusImovel.Disponivel)
            {
                Status = StatusImovel.Inativo;
                MarcarComoAtualizado();
            }
        }

        public void MarcarComoRascunho()
        {
            if (Status == StatusImovel.Disponivel || Status == StatusImovel.Inativo)
            {
                Status = StatusImovel.Rascunho;
                MarcarComoAtualizado();
            }
        }
    }
}
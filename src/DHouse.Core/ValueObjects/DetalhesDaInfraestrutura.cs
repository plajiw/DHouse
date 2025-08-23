namespace DHouse.Core.ValueObjects
{
    public record DetalhesDaInfraestrutura(
        bool Piscina = false,
        bool Academia = false,
        bool Churrasqueira = false,
        bool EspacoGourmet = false,
        bool SalaoDeFestas = false,
        bool SalaoDeJogos = false,
        bool Playground = false,
        bool Brinquedoteca = false,
        bool QuadraEsportiva = false,
        bool Sauna = false,
        bool Portaria24Horas = false,
        bool PortaoEletronico = false,
        bool Alarme = false,
        bool CercaEletrica = false,
        bool Elevador = false,
        bool Coworking = false,
        bool MercadoAutonomo = false,
        bool Bicicletario = false,
        bool PermiteAnimais = false,
        bool GasEncanado = false,
        bool GeradorDeEnergia = false,
        bool PocoArtesiano = false,
        bool Acessibilidade = false
    );
}
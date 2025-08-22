using System.ComponentModel;

namespace DHouse.Core
{
    public enum TipoImovel
    {
        // RESIDENCIAL
        [Description("Casa")]
        Casa,

        [Description("Casa de Condomínio")]
        CasaCondominio,

        [Description("Sobrado")]
        Sobrado,

        [Description("Apartamento")]
        Apartamento,

        [Description("Cobertura")]
        Cobertura,

        [Description("Flat")]
        Flat,

        [Description("Studio")]
        Studio,

        [Description("Kitnet")]
        Kitnet,

        [Description("Loft")]
        Loft,

        [Description("Duplex")]
        Duplex,

        [Description("Triplex")]
        Triplex,

        // COMERCIAL
        [Description("Loja")]
        Loja,

        [Description("Ponto Comercial")]
        PontoComercial,

        [Description("Sala Comercial")]
        SalaComercial,

        [Description("Conjunto Comercial")]
        ConjuntoComercial,

        [Description("Prédio Comercial")]
        PredioComercial,

        [Description("Box/Garagem Comercial")]
        BoxGaragem,

        // INDUSTRIAL
        [Description("Galpão")]
        Galpao,

        [Description("Barracão")]
        Barracao,

        [Description("Depósito")]
        Deposito,

        [Description("Área Industrial")]
        AreaIndustrial,

        // RURAL
        [Description("Chácara")]
        Chacara,

        [Description("Sítio")]
        Sitio,

        [Description("Fazenda")]
        Fazenda,

        [Description("Haras")]
        Haras,

        // TERRENOS
        [Description("Lote")]
        Lote,

        [Description("Loteamento")]
        Loteamento,

        [Description("Terreno")]
        Terreno,

        [Description("Área")]
        Area,

        // ESPECIAIS
        [Description("Hotel")]
        Hotel,

        [Description("Pousada")]
        Pousada,

        [Description("Andar Corporativo")]
        AndarCorporativo,

        [Description("Outro")]
        Outro
    }
}
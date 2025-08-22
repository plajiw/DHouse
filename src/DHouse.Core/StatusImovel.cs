using System.ComponentModel;

public enum StatusImovel
{
    [Description("Rascunho")]
    Rascunho,

    [Description("Disponível")]
    Disponivel,

    [Description("Em Negociação")]
    EmNegociacao,

    [Description("Reservado")]
    Reservado,

    [Description("Vendido")]
    Vendido,

    [Description("Alugado")]
    Alugado,

    [Description("Inativo")]
    Inativo
}
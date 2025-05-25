using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace bingo_api.src.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EPrizeType
    {
        [EnumMember(Value = "FourInLine")]
        FourInLine,         // Prêmio para quem acertar quatro números em uma única linha
        [EnumMember(Value = "FourCorners")]
        FourCorners,     // Prêmio para quem acertar os quatro cantos do cartão

        [EnumMember(Value = "SingleLine")]
        SingleLine,      // Prêmio para quem acertar uma linha

        [EnumMember(Value = "SingleColumn")]
        SingleColumn,    // Prêmio para quem acertar uma coluna

        [EnumMember(Value = "Diagonal")]
        Diagonal,        // Prêmio para quem acertar uma diagonal

        [EnumMember(Value = "InvertedDiagonal")]
        InvertedDiagonal,
        [EnumMember(Value = "DoubleLine")]
        DoubleLine,      // Prêmio para quem acertar duas linhas

        [EnumMember(Value = "DoubleColumn")]
        DoubleColumn,    // Prêmio para quem acertar duas colunas

        [EnumMember(Value = "FullCard")]
        FullCard,        // Prêmio para quem acertar o cartão cheio

        [EnumMember(Value = "TShape")]
        TShape,          // Prêmio para quem acertar o formato da letra T no cartão

        [EnumMember(Value = "XShape")]
        XShape,          // Prêmio para quem acertar o formato da letra X no cartão

        [EnumMember(Value = "PlusShape")]
        PlusShape,       // Prêmio para quem acertar o formato de um sinal de mais (+) no cartão

        [EnumMember(Value = "OuterEdge")]
        OuterEdge        // Prêmio para quem acertar toda a borda do cartão
    }
}

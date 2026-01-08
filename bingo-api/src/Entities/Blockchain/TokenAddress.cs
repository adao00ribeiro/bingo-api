using bingo_api.src.Entities.Shared;

namespace bingo_api.src.Entities.Blockchain;

public class TokenAddress : Entity
{
    public Guid TokenId { get; set; }
    public Guid NetworkId { get; set; }
    public string ContractAddress { get; set; } = null!;
    public Token Token { get; set; } = null!;
    public Network Network { get; set; } = null!;

    public TokenAddress(Guid tokenId, Guid networkId, string contractAddress)
    {
        TokenId = tokenId;
        NetworkId = networkId;
        ContractAddress = contractAddress;
    }
}

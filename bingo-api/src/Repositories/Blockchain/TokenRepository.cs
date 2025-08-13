using bingo_api.src.Context;
using bingo_api.src.Entities.Blockchain;
using bingo_api.src.Interfaces.blockchain;
using bingo_api.src.Repositories.Shared;

namespace bingo_api.src.Repositories.Blockchain;

public class TokenRepository : RepositoryBase<Token>, ITokenRepository
{
    public TokenRepository(DataContext dataContext) : base(dataContext)
    {
    }
}

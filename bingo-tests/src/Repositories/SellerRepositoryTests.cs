using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace bingo_tests.src.Repositories;

[Trait("Category", "RepositoriesTests")]
[CollectionDefinition("Postgres Test Collection")]
public class SellerRepositoryTests : IClassFixture<PostgresDbFixture>
{
    private readonly ISellerRepository _repository;

    public SellerRepositoryTests(PostgresDbFixture fixture)
    {

        _repository = fixture.ServiceProvider.GetRequiredService<ISellerRepository>();
    }


    [Fact]
    public async Task DeveCriarSeller()
    {
        var seller = new Seller
        {
            Email = "seller@seller.com",
            Cpf = "12345678901"
        };

        var id = await _repository.AddAsync(seller);
        var criado = await _repository.GetByIdAsync(id);
        Assert.NotNull(criado);
        Assert.Equal("seller@seller.com", criado.Email);
    }
}

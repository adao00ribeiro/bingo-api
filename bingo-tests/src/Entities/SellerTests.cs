using bingo_api.src.Context;
using bingo_api.src.Entities;


namespace bingo_tests.src.Entities;


[Trait("Category", "EntitiesTests")]
[Collection("Postgres Test Collection")]
public sealed class SellerTests 
{
    private readonly DataContext _context;

    public SellerTests(PostgresDbFixture fixture)
    {
        _context = fixture.Context;
    }

    [Fact]
    public async Task Can_Add_Seller()
    {
        // Arrange
        var seller = new Seller("seller@seller.com", "11111111111", DateTime.UtcNow, 0);
        // Act
        _context.Sellers.Add(seller);
        _context.SaveChanges();
        // Assert
        var result = _context.Sellers.First();
        Assert.Equal("seller@seller.com", result.Email);
    }
}

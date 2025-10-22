using bingo_api.src.Entities;
using bingo_api.src.Services.Prizes;
using Xunit.Abstractions;

namespace bingo_tests.src.Services.Prizes;

[Trait("Category", "PrizeTests")]
public class PrizeDiagonalServiceTest
{
    private readonly ITestOutputHelper _output;
    private readonly PrizeDiagonalService _service;
    private readonly Prize _prize;

    public PrizeDiagonalServiceTest(ITestOutputHelper output)
    {
        _output = output;
        _prize = new Prize(1000);
        _service = new PrizeDiagonalService(_prize);
    }
    [Fact]
    public void Execute3x5()
    {
        // Arrange
        var card = new Card
        {
            CardMarkedNumbers = new int[]
            {
                    0, 0, 0,0,0,
                    1, 1, 1,0,1,
                    0, 0, 0,0,0
            },
            Numbers = new int[]
            {
                    46, 54, 62,76,80,
                    18, 34, 35,69,88,
                    11, 12, 13,14,15
            },

            Punter = new Punter("faker@user", "Jodo joelso", "11111111111", DateTime.Now, Guid.NewGuid(),"")
        };

        var cards = new List<Card> { card };
        // Act
        _service.Execute(cards, 3, 5);

        Assert.Empty(_prize.WinningCards);
        // Assert.Equal("Player1", _prize.WinningCards.First().Punter);
        // Assert.Equal(1000M, _prize.WinningCards.First().ValueOfEachWinner);
    }
    [Fact]
    public void Execute5x5()
    {
        // Arrange
        var card = new Card
        {
            CardMarkedNumbers = new int[]
            {
                    1, 1, 1, 0, 0,
                    0, 1, 0, 0, 0,
                    0, 0, 1, 0, 0,
                    0, 0, 0, 1, 0,
                    1, 1, 1, 0, 1
            },
            Numbers = new int[]
            {
                    1, 2, 3, 4, 5,
                    6, 7, 8, 9, 10,
                    11, 12, 13, 14, 15,
                    16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25
            }
            ,

            Punter = new Punter("faker@user", "Jodo joelso", "11111111111", DateTime.Now, Guid.NewGuid(),"")

        };

        var cards = new List<Card> { card };

        // Act
        _service.Execute(cards, 5, 5);

        // Assert
        Assert.True(_prize.HasWinners(), "No winners were found when there should be.");
        Assert.NotEmpty(_prize.WinningCards);
        // Assert.Equal("Player1", _prize.WinningCards.First().Punter);
        // Assert.Equal(1000M, _prize.WinningCards.First().ValueOfEachWinner);
    }
    [Fact]
    public void Execute4x4()
    {
        // Arrange
        var card = new Card
        {
            CardMarkedNumbers = new int[]
            {
                    1, 1, 1, 1,
                    0, 1, 1, 0,
                    0, 1, 1, 0,
                    1, 0, 0, 1

            },
            Numbers = new int[]
            {
                    1, 2, 3, 4,
                    6, 7, 8, 9,
                    11, 12, 13, 14,
                    16, 17, 18, 19

            }
            ,

            Punter = new Punter("faker@user", "Jodo joelso", "11111111111", DateTime.Now, Guid.NewGuid(),"")

        };

        var cards = new List<Card> { card };

        // Act
        _service.Execute(cards, 4, 4);

        // Assert
        Assert.True(_prize.HasWinners(), "No winners were found when there should be.");
        Assert.NotEmpty(_prize.WinningCards);
        // Assert.Equal("Player1", _prize.WinningCards.First().Punter);
        // Assert.Equal(1000M, _prize.WinningCards.First().ValueOfEachWinner);
    }
    [Fact]
    public void Execute3x3()
    {
        // Arrange
        var card = new Card
        {
            CardMarkedNumbers = new int[]
            {
                    1, 0, 0,
                    0, 1, 0,
                    1, 0, 1,
            },
            Numbers = new int[]
            {
                    1, 2, 3,
                    6, 7, 8,
                    11, 12, 13,
            }
            ,

            Punter = new Punter("faker@user", "Jodo joelso", "11111111111", DateTime.Now, Guid.NewGuid(),"")

        };

        var cards = new List<Card> { card };

        // Act
        _service.Execute(cards, 3, 3);

        // Assert
        Assert.True(_prize.HasWinners(), "No winners were found when there should be.");
        Assert.NotEmpty(_prize.WinningCards);
        // Assert.Equal("Player1", _prize.WinningCards.First().Punter);
        // Assert.Equal(1000M, _prize.WinningCards.First().ValueOfEachWinner);
    }

}


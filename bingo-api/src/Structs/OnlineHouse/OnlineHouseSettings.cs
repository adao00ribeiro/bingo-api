namespace bingo_api.src.Structs.OnlineHouse;

public class OnlineHouseSettings
{
    public bool EnabledBingo { get; set; }
    public bool EnabledScratch { get; set; }
    public EmailOptions? EmailConfig { get; set; } = new EmailOptions();
    public BingoColorsConfig? BingoColorsConfig { get; set; } = new BingoColorsConfig();
}
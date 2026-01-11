namespace bingo_api.src.Structs;

public class OnlineHouseSettings
{
    public bool EnabledBingo { get; set; }
    public bool EnabledScratch { get; set; }
    public EmailOptions? EmailConfig { get; set; } = new EmailOptions();
}
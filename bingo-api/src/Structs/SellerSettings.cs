namespace bingo_api.src.Structs;

public class SellerSettings
{
    public EmailOptions? EmailConfig { get; set; } = new EmailOptions();
    public bool EnabledScratch{ get; set; }
}

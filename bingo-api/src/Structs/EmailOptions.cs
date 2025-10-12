
namespace bingo_api.src.Structs;

public class EmailOptions
{
    public SmtpSettings? PrimarySmtp { get; set; }
    public string? FromAddress { get; set; }
    public string? FromName { get; set; }

}

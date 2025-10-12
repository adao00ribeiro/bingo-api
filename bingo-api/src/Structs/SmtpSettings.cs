namespace bingo_api.src.Structs;

public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }

    public SmtpSettings(string host, int port, string user, string password)
    {
        Host = host;
        Port = port;
        User = user;
        Password = password;
        EnableSsl = true;
    }

}

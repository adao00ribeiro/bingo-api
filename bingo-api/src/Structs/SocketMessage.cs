


namespace bingo_api.src.Structs;

public class SocketMessage
{
    public string Command { get; set; }
    public string? Channel { get; set; }
    public object? Message { get; set; }
    public string Status { get; set; }

    public SocketMessage(string command, object message , string status)
    {
        Command = command;
        Channel = null; // Define como null
        Message = message;
        Status = status;
    }
    public SocketMessage(string command, string channel, object message,string status)
    {
        Command = command;
        Channel = channel;
        Message = message;
        Status = status;
    }
}

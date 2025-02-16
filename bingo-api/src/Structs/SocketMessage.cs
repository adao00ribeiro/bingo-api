


namespace bingo_api.src.Structs;

public class SocketMessage
{

    public string Command { get; set; }

    public string? Channel { get; set; }

    public object? Message { get; set; }

    public SocketMessage(string command, object message)
    {
        Command = command;
        Channel = null; // Define como null
        Message = message;
    }
    public SocketMessage(string command, string channel, object message)
    {
        Command = command;
        Channel = channel;
        Message = message;
    }
}

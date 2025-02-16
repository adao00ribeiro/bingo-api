using bingo_api.src.Structs;

namespace bingo_api.src.DTOs.Response;

public record SocketMessageResponseDto
{

    public string command { get; set; }

    public string? channel { get; set; }

    public object? message { get; set; }

    public SocketMessageResponseDto(string _command, string _channel, object _message)
    {
        this.command = _command;
        this.channel = _channel;
        this.message = _message;

    }
    internal static SocketMessageResponseDto ConvertToDto(SocketMessage message)
    {
        return new SocketMessageResponseDto(
        message.Command,
        message.Channel,
        message.Message
        );
    }
}

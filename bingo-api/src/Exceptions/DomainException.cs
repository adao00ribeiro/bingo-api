namespace bingo_api.src.Exceptions;

public sealed class DomainException(string message) : Exception(message);
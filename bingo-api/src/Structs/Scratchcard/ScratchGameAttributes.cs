namespace bingo_api.src.Structs.Scratchcard;

public record ScratchGameAttributes
{
     public List<ScratchPayout> PayoutTable { get; set; } = new();
}

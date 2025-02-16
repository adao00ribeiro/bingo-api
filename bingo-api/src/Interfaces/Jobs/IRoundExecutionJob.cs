namespace bingo_api.src.Interfaces.Jobs;

public interface IRoundExecutionJob
{
    Task Execute(Guid round_id);
}

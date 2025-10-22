using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces;

namespace bingo_api.src.Infrastructure;

public class EventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchEventsAsync(IEnumerable<Entity> entities)
    {
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            // Obtém o tipo do handler correspondente (ex: IDomainEventHandler<ScratchPrizeCreatedEvent>)
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            // Resolve todos os handlers registrados para esse evento
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                // Invoca HandleAsync(domainEvent)
                var handleMethod = handlerType.GetMethod("HandleAsync");
                if (handleMethod != null)
                {
                    var task = (Task?)handleMethod.Invoke(handler, new object[] { domainEvent });
                    if (task != null)
                        await task.ConfigureAwait(false);
                }
            }
        }

        // Limpa os eventos após o processamento
        foreach (var entity in entities)
            entity.ClearDomainEvents();
    }
}

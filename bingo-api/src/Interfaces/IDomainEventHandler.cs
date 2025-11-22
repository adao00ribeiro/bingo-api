using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.Interfaces;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}
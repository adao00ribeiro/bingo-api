using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bingo_api.src.Interfaces.Jobs;

public interface IShowTimelineStepJob
{
    Task Execute(Guid roundId);
}

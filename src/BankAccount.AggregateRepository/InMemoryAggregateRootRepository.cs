using System.Collections.ObjectModel;
using EventSourcing.Aggregate;
using EventSourcing.Repository;

namespace BankAccount.IAggregateRepository;

public class InMemoryAggregateRootRepository<TAggregate> : IAggregateRootRepository<TAggregate>
    where TAggregate : AggregateRoot, new()
{
    public ReadOnlyCollection<IEvent> Events => _events.AsReadOnly();
    private readonly List<IEvent> _events = [];
    
    public async Task<TAggregate?> FindByIdAsync(Guid aggregateId)
    {
        var @events = _events.Where(e => e.AggregateId == aggregateId).ToList();
        if(events.Count == 0)
        {
            return default;
        }
        var aggregate = AggregateFactory<TAggregate>.Create(new AggregateId(aggregateId));
        aggregate.LoadFromHistory(events);
        await Task.CompletedTask;
        return aggregate;
    }

    public async Task SaveAsync(TAggregate? aggregate)
    {
        if (aggregate is null)
            return;
        var uncommittedEvents = aggregate.GetUncommittedEvents().ToList();
        if (uncommittedEvents.Count == 0)
        {
            return;
        }

        _events.AddRange(uncommittedEvents);
        aggregate.ClearUncommittedEvents();
        await Task.CompletedTask;
    }
}


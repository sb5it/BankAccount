using EventSourcing.Aggregate;

namespace BankAccount.Events;

public class BalanceIncreasingEvent(decimal amount, Guid aggregateId) : Event(aggregateId, Guid.NewGuid())
{
    public decimal Amount { get; } = amount;
}
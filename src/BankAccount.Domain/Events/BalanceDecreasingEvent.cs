using EventSourcing.Aggregate;

namespace BankAccount.Events;

public class BalanceDecreasingEvent(decimal amount, Guid aggregateId) : Event(aggregateId, Guid.NewGuid())
{
    public decimal Amount { get; } = amount;
}
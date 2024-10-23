using EventSourcing.Aggregate;

namespace BankAccount.Events;

public class BalanceDecreased(decimal amount, Guid aggregateId) : Event(aggregateId, Guid.NewGuid())
{
    public decimal Amount { get; } = amount;
}

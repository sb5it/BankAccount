using EventSourcing.Aggregate;

namespace BankAccount.Events;

public class BalanceIncreased(decimal amount, Guid aggregateId) : Event(aggregateId, Guid.NewGuid())
{
    public decimal Amount { get; } = amount;
}

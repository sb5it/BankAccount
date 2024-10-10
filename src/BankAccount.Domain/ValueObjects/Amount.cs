using EventSourcing.Aggregate;
using EventSourcing.ValueObject;

namespace BankAccount.ValueObjects;

public class Amount(decimal value) : IValueObject
{
    public decimal Value { get; } = value;

    public Amount Add(Amount amount) => new(Value + amount.Value);
}

public class AccountId(Guid value) : AggregateId(value);
using BankAccount.Events;
using BankAccount.ValueObjects;
using EventSourcing.Aggregate;

namespace BankAccount;

public class Account : AggregateRoot
{
    public Amount Balance { get; private set; } = null!;

    public void CreateAccount(AggregateId id, Amount initialBalance)
    {
        RaiseDomainEvent(new AccountCreated(initialBalance.Value, id.Id));
    }
    
    public void Deposit(Amount amount)
    {
        if(Balance.Value + amount.Value < 0)
            throw new InvalidOperationException("Insufficient funds");
        if(amount.Value > 0)
            RaiseDomainEvent(new BalanceIncreased(amount.Value, Id.Id));
        else
            RaiseDomainEvent(new BalanceDecreased(amount.Value, Id.Id));
    }
    
    private void When(AccountCreated e)
    {
        Balance = new Amount(e.Amount);
    }
    
    private void When(BalanceIncreased e)
    {
        Balance = Balance.Add(new Amount(e.Amount));
    }
    
    private void When(BalanceDecreased e)
    {
        Balance = Balance.Add(new Amount(e.Amount));
    }
}

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
            RaiseDomainEvent(new BalanceIncreasingEvent(amount.Value, Id.Id));
        else
            RaiseDomainEvent(new BalanceDecreasingEvent(amount.Value, Id.Id));
    }
    
    private void When(AccountCreated e)
    {
        Balance = new Amount(e.Amount);
    }
    
    private void When(BalanceIncreasingEvent e)
    {
        Balance = Balance.Add(new Amount(e.Amount));
    }
    
    private void When(BalanceDecreasingEvent e)
    {
        Balance = Balance.Add(new Amount(e.Amount));
    }
}
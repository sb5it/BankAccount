using BankAccount.Events;
using BankAccount.ValueObjects;
using EventSourcing.Aggregate;
using FluentAssertions;
using Xunit;

namespace BankAccount.Domain.Tests;

public class AccountTests
{
    private static readonly AggregateId AccountId = new(Guid.NewGuid());
    private readonly Amount _initialBalance = new(100);

    [Fact]
    public void CreateAccount_ShouldRaiseAccountCreatedEvent()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        account.CreateAccount(AccountId, _initialBalance);
        
        var uncommittedEvents = account.GetUncommittedEvents();
        uncommittedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AccountCreated>()
            .Which.Amount.Should().Be(_initialBalance.Value);

        var accountCreatedEvent = (AccountCreated)uncommittedEvents.Single();
        accountCreatedEvent.AggregateId.Should().Be(AccountId.Id);
    }
    
    [Fact]
    public void When_AccountCreated_ShouldSetBalance()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, AccountId.Id);
        
        account.LoadFromHistory(new List<IEvent> { accountCreatedEvent });
        
        account.Balance.Value.Should().Be(_initialBalance.Value);
    }

    [Fact]
    public void When_BalanceIncreased_ShouldIncreaseBalance()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, AccountId.Id);
        var balanceIncreased = new BalanceIncreased(50, AccountId.Id);

        account.LoadFromHistory(new List<IEvent> { accountCreatedEvent, balanceIncreased });

        account.Balance.Value.Should().Be(150); // 100 (initial) + 50 (increase)
    }

    [Fact]
    public void When_BalanceDecreased_ShouldDecreaseBalance()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, AccountId.Id);
        var balanceDecreased = new BalanceDecreased(-30, AccountId.Id);

        account.LoadFromHistory(new List<IEvent> { accountCreatedEvent, balanceDecreased });

        account.Balance.Value.Should().Be(70); // 100 (initial) - 30 (decrease)
    }

    [Fact]
    public void Deposit_PositiveAmount_ShouldRaiseBalanceIncreased()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, AccountId.Id);
        account.LoadFromHistory(new List<IEvent> { accountCreatedEvent });
        var depositAmount = new Amount(50);

        account.Deposit(depositAmount);
        
        var uncommittedEvents = account.GetUncommittedEvents();
        uncommittedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<BalanceIncreased>()
            .Which.Amount.Should().Be(depositAmount.Value);
    }

    [Fact]
    public void Deposit_NegativeAmount_ShouldRaiseBalanceDecreased()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, AccountId.Id);
        account.LoadFromHistory(new List<IEvent> { accountCreatedEvent });
        var withdrawalAmount = new Amount(-30);

        account.Deposit(withdrawalAmount);

        var uncommittedEvents = account.GetUncommittedEvents();
        uncommittedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<BalanceDecreased>()
            .Which.Amount.Should().Be(withdrawalAmount.Value);
    }

    [Fact]
    public void Deposit_InsufficientFunds_ShouldThrowInvalidOperationException()
    {
        var account = AggregateFactory<Account>.Create(AccountId);
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, AccountId.Id);
        account.LoadFromHistory(new List<IEvent> { accountCreatedEvent });
        var excessiveWithdrawal = new Amount(-150);

        var act = () => account.Deposit(excessiveWithdrawal);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Insufficient funds");
    }
}

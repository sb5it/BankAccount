using BankAccount.Events;
using BankAccount.ValueObjects;
using EventSourcing.Aggregate;
using FluentAssertions;
using Xunit;

namespace BankAccount.Domain.Tests;

public class AccountTests
{
    private readonly Account _account = new();
    private readonly AggregateId _accountId = new(Guid.NewGuid());
    private readonly Amount _initialBalance = new(100);

    [Fact]
    public void CreateAccount_ShouldRaiseAccountCreatedEvent()
    {
        // Act
        _account.CreateAccount(_accountId, _initialBalance);
        
        // Assert
        var uncommittedEvents = _account.GetUncommittedEvents();
        uncommittedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<AccountCreated>()
            .Which.Amount.Should().Be(_initialBalance.Value);

        var accountCreatedEvent = (AccountCreated)uncommittedEvents.GetEnumerator().Current;
        accountCreatedEvent.Id.Should().Be(_accountId.Id);
    }

    [Fact]
    public void Deposit_PositiveAmount_ShouldRaiseBalanceIncreasingEvent()
    {
        // Arrange
        _account.CreateAccount(_accountId, _initialBalance);
        _account.ClearUncommittedEvents();

        var depositAmount = new Amount(50);

        // Act
        _account.Deposit(depositAmount);
        
        // Assert
        var uncommittedEvents = _account.GetUncommittedEvents();
        uncommittedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<BalanceIncreasingEvent>()
            .Which.Amount.Should().Be(depositAmount.Value);
    }

    [Fact]
    public void Deposit_NegativeAmount_ShouldRaiseBalanceDecreasingEvent()
    {
        // Arrange
        _account.CreateAccount(_accountId, _initialBalance);
        _account.ClearUncommittedEvents();

        var withdrawalAmount = new Amount(-30);

        // Act
        _account.Deposit(withdrawalAmount);

        // Assert
        var uncommittedEvents = _account.GetUncommittedEvents();
        uncommittedEvents.Should().ContainSingle()
            .Which.Should().BeOfType<BalanceDecreasingEvent>()
            .Which.Amount.Should().Be(withdrawalAmount.Value);
    }

    [Fact]
    public void Deposit_InsufficientFunds_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _account.CreateAccount(_accountId, _initialBalance);
        var excessiveWithdrawal = new Amount(-150);

        // Act
        Action act = () => _account.Deposit(excessiveWithdrawal);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Insufficient funds");
    }

    [Fact]
    public void When_AccountCreated_ShouldSetBalance()
    {
        // Arrange
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, _accountId.Id);
        
        // Act
        _account.LoadFromHistory(new List<IEvent> { accountCreatedEvent });
        
        // Assert
        _account.Balance.Value.Should().Be(_initialBalance.Value);
    }

    [Fact]
    public void When_BalanceIncreasingEvent_ShouldIncreaseBalance()
    {
        // Arrange
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, _accountId.Id);
        var balanceIncreasingEvent = new BalanceIncreasingEvent(50, _accountId.Id);

        // Act
        _account.LoadFromHistory(new List<IEvent> { accountCreatedEvent, balanceIncreasingEvent });

        // Assert
        _account.Balance.Value.Should().Be(150); // 100 (initial) + 50 (increase)
    }

    [Fact]
    public void When_BalanceDecreasingEvent_ShouldDecreaseBalance()
    {
        // Arrange
        var accountCreatedEvent = new AccountCreated(_initialBalance.Value, _accountId.Id);
        var balanceDecreasingEvent = new BalanceDecreasingEvent(-30, _accountId.Id);

        // Act
        _account.LoadFromHistory(new List<IEvent> { accountCreatedEvent, balanceDecreasingEvent });

        // Assert
        _account.Balance.Value.Should().Be(70); // 100 (initial) - 30 (decrease)
    }
}
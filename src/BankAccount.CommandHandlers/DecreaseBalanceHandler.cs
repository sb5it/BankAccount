using BankAccount.Commands;
using BankAccount.ValueObjects;
using EventSourcing.CommandHandler;
using EventSourcing.Repository;

namespace BankAccount.CommandHandlers;

public class DecreaseBalanceHandler(IAggregateRootRepository<Account> repository)
    : AggregateCommandHandler<DecreaseBalance, Account>(repository)
{
    protected override Task ProcessAsync(DecreaseBalance command)
    {
        AggregateRoot?.Deposit(new Amount(command.Amount));
        return Task.CompletedTask;
    }
}
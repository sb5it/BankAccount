using BankAccount.Commands;
using BankAccount.ValueObjects;
using EventSourcing.CommandHandler;
using EventSourcing.Repository;

namespace BankAccount.CommandHandlers;

public class IncreaseBalanceHandler(IAggregateRootRepository<Account> repository)
    : AggregateCommandHandler<IncreaseBalance, Account>(repository)
{
    protected override Task ProcessAsync(IncreaseBalance command)
    {
        AggregateRoot?.Deposit(new Amount(command.Amount));
        return Task.CompletedTask;
    }
}
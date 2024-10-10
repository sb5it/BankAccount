using BankAccount.Commands;
using BankAccount.ValueObjects;
using EventSourcing.Aggregate;
using EventSourcing.CommandHandler;
using EventSourcing.Repository;

namespace BankAccount.CommandHandlers;

public class CreateAccountHandler(IAggregateRootRepository<Account> repository)
    : AggregateCommandHandler<CreateAccount, Account>(repository)
{
    protected override Task ProcessAsync(CreateAccount command)
    {
        AggregateRoot ??= AggregateFactory<Account>.Create(new AccountId(command.Id));
        AggregateRoot.CreateAccount(new AccountId(command.Id),new Amount(command.InitialAmount));
        return Task.CompletedTask;
    }
}
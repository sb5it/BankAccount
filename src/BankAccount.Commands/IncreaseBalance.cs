using EventSourcing.Command;

namespace BankAccount.Commands;

public class IncreaseBalance : ICommand
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
}
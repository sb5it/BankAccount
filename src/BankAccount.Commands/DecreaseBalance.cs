using EventSourcing.Command;

namespace BankAccount.Commands;

public class DecreaseBalance : ICommand
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
}
using EventSourcing.Command;

namespace BankAccount.Commands;

public class CreateAccount : ICommand
{
    public Guid Id { get; set; }
    public decimal InitialAmount { get; set; }
}
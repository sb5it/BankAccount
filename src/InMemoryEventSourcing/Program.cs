using BankAccount;
using BankAccount.CommandHandlers;
using BankAccount.Commands;
using BankAccount.IAggregateRepository;
using EventSourcing.CommandHandler;
using EventSourcing.Repository;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        
        ConfigureServices(serviceCollection);
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var createAccountCommand = new CreateAccount
        {
            Id = Guid.NewGuid(),
            InitialAmount = 100
        };
        var createAccountHandler = serviceProvider.GetService<AggregateCommandHandler<CreateAccount, Account>>();
        await createAccountHandler.HandleAsync(createAccountCommand);
        
        var increaseBalanceCommand = new IncreaseBalance
        {
            Id = createAccountCommand.Id,
            Amount = 100
        };
        var increaseBalanceHandler = serviceProvider.GetService<AggregateCommandHandler<IncreaseBalance, Account>>();
        increaseBalanceHandler.HandleAsync(increaseBalanceCommand);
        
        var decreaseBalanceCommand = new DecreaseBalance
        {
            Id = createAccountCommand.Id,
            Amount = -10
        };
        var decreaseBalanceHandler = serviceProvider.GetService<AggregateCommandHandler<DecreaseBalance, Account>>();
        decreaseBalanceHandler.HandleAsync(decreaseBalanceCommand);

        Console.ReadKey();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAggregateRootRepository<Account>, InMemoryAggregateRootRepository<Account>>();
        services.AddTransient<AggregateCommandHandler<CreateAccount, Account>, CreateAccountHandler>();
        services.AddTransient<AggregateCommandHandler<IncreaseBalance, Account>, IncreaseBalanceHandler>();
        services.AddTransient<AggregateCommandHandler<DecreaseBalance, Account>, DecreaseBalanceHandler>();
    }
}

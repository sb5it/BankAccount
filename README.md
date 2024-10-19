
## Overview

This repository contains an imaginary bank account project built using Domain-Driven Design (DDD) principles and event sourcing. The core idea is to model a bank account system that tracks domain events like account creation, balance increase, and balance decrease using an in-memory event store.

## Project Structure

The solution is organized into several projects to adhere to DDD principles and separate different responsibilities:

- **BankAccount.AggregateRepository**: Contains repository implementations for managing aggregate roots.
- **BankAccount.CommandHandlers**: Includes handlers for commands that modify the state of the bank account, such as `CreateAccountHandler`, `IncreaseBalanceHandler`, and `DecreaseBalanceHandler`.
- **BankAccount.Commands**: Defines the commands like `CreateAccount`, `IncreaseBalance`, and `DecreaseBalance` that represent intentions to change the state.
- **BankAccount.Domain**: Contains the domain logic, including:
  - **Events**: Represents changes in the state, such as `AccountCreated`, `BalanceDecreasingEvent`, and `BalanceIncreasingEvent`.
  - **ValueObjects**: Defines value objects like `Amount` and `Account`.
- **InMemoryEventSourcing**: Provides an in-memory event store implementation that stores domain events.
- **tests/BankAccount.Domain.Tests**: Includes unit tests for domain logic to ensure the integrity of the business rules.

## Key Features

- **Event Sourcing**: The state of the bank account is derived from a sequence of domain events, which are stored using an in-memory event store.
- **Command Handlers**: Uses command handlers to process commands and generate corresponding events.
- **Domain-Driven Design**: Separates concerns and encapsulates business rules and logic within the domain layer.

## How to Run

1. Clone the repository:

```bash
   git clone https://github.com/your-username/BankAccount.git
   cd BankAccount
```
2. Build the solution:
```bash
    dotnet build BankAccount.sln
```
3. Run the application:
```bash
    dotnet run --project src/InMemoryEventSourcing`
```
4. Run the tests:
    
```bash
    dotnet test tests/BankAccount.Domain.Tests
```
    

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.


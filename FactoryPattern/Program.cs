using System;

namespace FactoryPattern
{
    /*
     * This program demonstrates the Factory Pattern implementation in C#.
     * 
     * Key concepts demonstrated:
     * 1. Simple Factory: A single class with a method that creates objects based on parameters
     * 2. Factory Method Pattern: Creates objects through inheritance and method overriding
     * 3. Abstract Factory Pattern: Creates families of related objects
     * 
     * SOLID Principles demonstrated:
     * 1. Single Responsibility Principle (SRP):
     *    - Each processor class is responsible only for its specific payment method
     *    - Factory classes are solely responsible for object creation
     * 
     * 2. Open/Closed Principle (OCP):
     *    - New payment processors can be added without modifying existing client code
     *    - The system is open for extension but closed for modification
     * 
     * 3. Liskov Substitution Principle (LSP):
     *    - All payment processors can be used interchangeably through the IPaymentProcessor interface
     *    - Client code works with the abstraction, not concrete implementations
     * 
     * 4. Interface Segregation Principle (ISP):
     *    - Interfaces are specific to the needs of the clients
     *    - The IPaymentProcessor interface contains only methods needed by all processors
     * 
     * 5. Dependency Inversion Principle (DIP):
     *    - Client code depends on abstractions (interfaces) not concrete implementations
     *    - High-level modules (client code) are not dependent on low-level modules (specific processors)
     */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Factory Pattern Examples ===\n");

            // Example 1: Simple Factory
            Console.WriteLine("--- Simple Factory Example ---");
            DemoSimpleFactory();

            // Example 2: Abstract Factory
            Console.WriteLine("\n--- Abstract Factory Example ---");
            DemoAbstractFactory();

            // Wait for user input
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void DemoSimpleFactory()
        {
            // Using the simple factory to create payment processors
            var creditCardProcessor = PaymentProcessorFactory.CreateProcessor(PaymentMethod.CreditCard);
            var payPalProcessor = PaymentProcessorFactory.CreateProcessor(PaymentMethod.PayPal);
            var bankTransferProcessor = PaymentProcessorFactory.CreateProcessor(PaymentMethod.BankTransfer);

            // Process payments using different processors
            ProcessPayment(creditCardProcessor, 99.99m);
            ProcessPayment(payPalProcessor, 149.99m);
            ProcessPayment(bankTransferProcessor, 999.99m);
        }

        static void DemoAbstractFactory()
        {
            // Getting factories for each payment method using the factory provider
            var creditCardFactory = PaymentFactoryProvider.GetFactory(PaymentMethod.CreditCard);
            var payPalFactory = PaymentFactoryProvider.GetFactory(PaymentMethod.PayPal);
            var bankTransferFactory = PaymentFactoryProvider.GetFactory(PaymentMethod.BankTransfer);

            // Creating processors using the factories
            var creditCardProcessor = creditCardFactory.CreateProcessor();
            var payPalProcessor = payPalFactory.CreateProcessor();
            var bankTransferProcessor = bankTransferFactory.CreateProcessor();

            // Process payments using different processors
            ProcessPayment(creditCardProcessor, 199.99m);
            ProcessPayment(payPalProcessor, 249.99m);
            ProcessPayment(bankTransferProcessor, 1999.99m);
        }

        static void ProcessPayment(IPaymentProcessor processor, decimal amount)
        {
            Console.WriteLine($"\nUsing {processor.GetPaymentMethod()} processor:");
            bool result = processor.ProcessPayment(amount);
            Console.WriteLine($"Payment {(result ? "succeeded" : "failed")}");
        }
    }
}
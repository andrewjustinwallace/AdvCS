using System;

namespace FactoryPattern
{
    // Product interface
    public interface IPaymentProcessor
    {
        bool ProcessPayment(decimal amount);
        string GetPaymentMethod();
    }

    // Concrete Products
    public class CreditCardProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of ${amount}");
            // Implementation logic for credit card payment
            return true;
        }

        public string GetPaymentMethod() => "Credit Card";
    }

    public class PayPalProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of ${amount}");
            // Implementation logic for PayPal payment
            return true;
        }

        public string GetPaymentMethod() => "PayPal";
    }

    public class BankTransferProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing bank transfer of ${amount}");
            // Implementation logic for bank transfer
            return true;
        }

        public string GetPaymentMethod() => "Bank Transfer";
    }

    // Payment Method Enum
    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        BankTransfer
    }

    // Factory class
    public class PaymentProcessorFactory
    {
        // Simple Factory method
        public static IPaymentProcessor CreateProcessor(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.CreditCard => new CreditCardProcessor(),
                PaymentMethod.PayPal => new PayPalProcessor(),
                PaymentMethod.BankTransfer => new BankTransferProcessor(),
                _ => throw new ArgumentException($"Payment method {method} is not supported")
            };
        }
    }

    // Abstract Factory Interface
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor CreateProcessor();
    }

    // Concrete factories for each payment type
    public class CreditCardProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor CreateProcessor() => new CreditCardProcessor();
    }

    public class PayPalProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor CreateProcessor() => new PayPalProcessor();
    }

    public class BankTransferProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor CreateProcessor() => new BankTransferProcessor();
    }

    // Factory Provider (Factory of Factories)
    public class PaymentFactoryProvider
    {
        public static IPaymentProcessorFactory GetFactory(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.CreditCard => new CreditCardProcessorFactory(),
                PaymentMethod.PayPal => new PayPalProcessorFactory(),
                PaymentMethod.BankTransfer => new BankTransferProcessorFactory(),
                _ => throw new ArgumentException($"Payment method {method} is not supported")
            };
        }
    }
}
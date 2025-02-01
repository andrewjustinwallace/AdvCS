/*
 * This program demonstrates the Strategy design pattern using a payment processing example.
 * The Strategy pattern defines a family of algorithms, encapsulates each one, and makes
 * them interchangeable. It lets the algorithm vary independently from clients that use it.
 * 
 * Key concepts demonstrated:
 * 1. Strategy Interface (IPaymentStrategy)
 * 2. Concrete Strategies (CreditCardPayment, PayPalPayment, CryptoPayment)
 * 3. Context (ShoppingCart) that uses the strategy
 * 4. Runtime strategy selection
 * 
 * Pattern benefits:
 * - Encapsulates algorithm implementations
 * - Enables runtime algorithm switching
 * - Eliminates conditional statements
 * - Promotes code reuse
 * - Separates algorithm from usage context
 */

using System;

// Strategy Interface
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

// Concrete Strategies
public class CreditCardPayment : IPaymentStrategy
{
    private readonly string _cardNumber;
    private readonly string _name;

    public CreditCardPayment(string cardNumber, string name)
    {
        _cardNumber = cardNumber;
        _name = name;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid ${amount} using Credit Card ({_cardNumber})");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    private readonly string _email;

    public PayPalPayment(string email)
    {
        _email = email;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid ${amount} using PayPal account ({_email})");
    }
}

public class CryptoPayment : IPaymentStrategy
{
    private readonly string _walletId;
    private readonly string _currency;

    public CryptoPayment(string walletId, string currency)
    {
        _walletId = walletId;
        _currency = currency;
    }

    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid ${amount} worth of {_currency} using wallet {_walletId}");
    }
}

// Context
public class ShoppingCart
{
    private IPaymentStrategy _paymentStrategy;
    private decimal _total;

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
    }

    public void AddItem(decimal price)
    {
        _total += price;
    }

    public void Checkout()
    {
        if (_paymentStrategy == null)
        {
            throw new InvalidOperationException("Please select a payment method.");
        }
        _paymentStrategy.Pay(_total);
        _total = 0; // Reset cart
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create shopping cart
        var cart = new ShoppingCart();

        // Add some items
        cart.AddItem(100);
        cart.AddItem(50);

        // Pay with Credit Card
        Console.WriteLine("Paying with Credit Card:");
        cart.SetPaymentStrategy(new CreditCardPayment("1234-5678-9012-3456", "John Doe"));
        cart.Checkout();

        // Add more items
        cart.AddItem(75);

        // Pay with PayPal
        Console.WriteLine("\nPaying with PayPal:");
        cart.SetPaymentStrategy(new PayPalPayment("john.doe@email.com"));
        cart.Checkout();

        // Add more items
        cart.AddItem(200);

        // Pay with Cryptocurrency
        Console.WriteLine("\nPaying with Cryptocurrency:");
        cart.SetPaymentStrategy(new CryptoPayment("0x123...abc", "BTC"));
        cart.Checkout();

        Console.ReadLine();
    }
}
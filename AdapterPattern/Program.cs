/*
 * This program demonstrates the Adapter design pattern in C#.
 * The Adapter pattern allows objects with incompatible interfaces to work together
 * by wrapping an object in an adapter that makes it compatible with another interface.
 * 
 * Key concepts demonstrated:
 * 1. Interface-based design with ITarget
 * 2. Adaptee class with incompatible interface
 * 3. Adapter class that bridges ITarget and Adaptee
 * 4. Client code working with ITarget interface
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Adapter class has single responsibility of adapting one interface to another
 *    - Adaptee maintains its original responsibility without modification
 *    - Client maintains its responsibility of using the Target interface
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New adapters can be created without modifying existing code
 *    - Existing adaptee classes can be reused without modification
 *    - Client code remains unchanged when new adapters are added
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Adapter can be used anywhere the Target interface is expected
 *    - Multiple adapters for the same interface can be substituted
 *    - Runtime behavior remains consistent with interface contract
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Target interface defines minimal required methods
 *    - Adapter only implements necessary interface methods
 *    - Client only depends on methods it actually uses
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - High-level client code depends on Target abstraction
 *    - Adapter provides loose coupling between interfaces
 *    - Dependencies flow toward abstractions, not concrete classes
 * 
 * Pattern benefits:
 * - Allows integration of new code with existing code
 * - Promotes loose coupling
 * - Enables reusability of existing functionality
 * - Provides a way to retrofit existing code to work with new interfaces
 */

// The interface that the client expects to work with
public interface ITarget
{
    string GetRequest();
}

// The interface that needs to be adapted
public class Adaptee
{
    public string GetSpecificRequest()
    {
        return "Specific request.";
    }
}

// The Adapter class
public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }

    public string GetRequest()
    {
        return $"Adapter: {_adaptee.GetSpecificRequest()}";
    }
}

// Client code
public class Client
{
    public void MakeRequest(ITarget target)
    {
        Console.WriteLine(target.GetRequest());
    }
}

// Usage
class Program
{
    static void Main(string[] args)
    {
        Adaptee adaptee = new Adaptee();
        ITarget target = new Adapter(adaptee);

        Client client = new Client();
        client.MakeRequest(target);
    }
}
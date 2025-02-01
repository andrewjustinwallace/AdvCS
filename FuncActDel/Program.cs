/*
 * This program demonstrates the use of Func and Action delegates in C#.
 * It shows how these built-in delegate types can be used to create more flexible
 * and reusable code by passing methods as parameters.
 *
 * Key concepts demonstrated:
 * 1. Func<T, TResult> - delegates that return a value
 *    - Takes input parameters and returns a result
 *    - Last type parameter is always the return type
 * 
 * 2. Action<T> - delegates that don't return a value
 *    - Takes input parameters but returns void
 *    - Used for performing operations with side effects
 *
 * 3. Lambda Expressions
 *    - Inline method definitions
 *    - Both statement and expression lambdas
 *
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each delegate handles one specific operation
 *    - PerformOperation method focuses solely on executing the operation
 *    - ProcessString method handles only string processing delegation
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New operations can be added without modifying existing methods
 *    - Delegate parameters allow extension of functionality
 *    - Base methods remain unchanged while supporting new behaviors
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Any compatible delegate can be substituted
 *    - Lambda expressions can be used interchangeably with method delegates
 *    - Behavior remains consistent with delegate signatures
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Func and Action delegates provide focused interfaces
 *    - Methods depend only on the delegate types they need
 *    - Clients aren't forced to implement unnecessary methods
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Methods depend on delegate abstractions
 *    - Implementation details are passed as parameters
 *    - High-level modules are decoupled from specific implementations
 *
 * Common use cases:
 * - Callback methods
 * - LINQ operations
 * - Event handlers
 * - Strategy pattern implementation
 */

public class DelegateExample
{
    // Method that takes a Func<int, int, int> delegate
    public static int PerformOperation(int a, int b, Func<int, int, int> operation)
    {
        return operation(a, b);
    }

    // Method that takes an Action<string> delegate
    public static void ProcessString(string input, Action<string> processor)
    {
        processor(input);
    }

    public static void Main()
    {
        // Using Func<> delegate
        Func<int, int, int> add = (x, y) => x + y;
        Func<int, int, int> multiply = (x, y) => x * y;

        Console.WriteLine($"Addition: {PerformOperation(5, 3, add)}");
        Console.WriteLine($"Multiplication: {PerformOperation(5, 3, multiply)}");

        // Using lambda expressions directly
        Console.WriteLine($"Subtraction: {PerformOperation(5, 3, (x, y) => x - y)}");

        // Using Action<> delegate
        Action<string> printUpper = s => Console.WriteLine(s.ToUpper());
        Action<string> printLower = s => Console.WriteLine(s.ToLower());

        ProcessString("Hello, World!", printUpper);
        ProcessString("Hello, World!", printLower);

        // Using lambda expression directly
        ProcessString("Hello, World!", s => Console.WriteLine(s.Length));
    }
}
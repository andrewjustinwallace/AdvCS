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
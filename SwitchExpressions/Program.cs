/*
 * This program demonstrates the use of Switch Expressions in C# 8.0 and later.
 * It showcases various ways to use the new switch expression syntax for pattern matching
 * and value-based switching.
 * 
 * Key concepts demonstrated:
 * 1. Basic switch expressions
 * 2. Pattern matching
 * 3. Property patterns
 * 4. Tuple patterns
 * 5. Relational patterns
 * 6. Logical patterns (and/or)
 * 7. Guard clauses (when conditions)
 * 
 * Examples include:
 * - Day type classification (weekend/weekday)
 * - Object type and value pattern matching
 * - Point coordinate classification
 * - Person age group classification
 * - Shape drawing function selection
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("C# Switch Expression Examples\n");

        // Example 1: Basic switch expression
        Console.WriteLine("1. Day Type:");
        Console.WriteLine($"Monday is a {GetDayType(DayOfWeek.Monday)}");
        Console.WriteLine($"Saturday is a {GetDayType(DayOfWeek.Saturday)}");

        // Example 2: Switch expression with pattern matching
        Console.WriteLine("\n2. Object Classification:");
        Console.WriteLine(Classify(""));
        Console.WriteLine(Classify("Hello"));
        Console.WriteLine(Classify(-5));
        Console.WriteLine(Classify(10));
        Console.WriteLine(Classify(null));
        Console.WriteLine(Classify(3.14));

        // Example 3: Switch expression with tuple patterns
        Console.WriteLine("\n3. Point Classification:");
        Console.WriteLine(ClassifyPoint(0, 0));
        Console.WriteLine(ClassifyPoint(1, 1));
        Console.WriteLine(ClassifyPoint(2, 3));
        Console.WriteLine(ClassifyPoint(4, -2));

        // Example 4: Switch expression with property patterns
        Console.WriteLine("\n4. Person Description:");
        Console.WriteLine(Describe(new Person("Alice", 15)));
        Console.WriteLine(Describe(new Person("Bob", 30)));
        Console.WriteLine(Describe(new Person("Charlie", 70)));

        // Calling functions
        Console.WriteLine("Enter a shape (circle, square, or triangle):");
        string shape = Console.ReadLine().ToLower();

        // Using switch expression to execute a function
        _ = shape switch
        {
            "circle" => DrawCircle(),
            "square" => DrawSquare(),
            "triangle" => DrawTriangle(),
            _ => Unknown()
        };
    }

    static string GetDayType(DayOfWeek day) => day switch
    {
        DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend",
        _ => "Weekday"
    };

    static string Classify(object obj) => obj switch
    {
        string s when s.Length == 0 => "Empty string",
        string s => $"String of length {s.Length}",
        int i when i < 0 => "Negative integer",
        int i => $"Positive integer: {i}",
        null => "Null object",
        _ => "Unknown type"
    };

    static string ClassifyPoint(int x, int y) => (x, y) switch
    {
        (0, 0) => "Origin",
        (var a, var b) when a == b => "On diagonal",
        (_, > 0) => "Above X-axis",
        (_, < 0) => "Below X-axis",
        _ => "On X-axis"
    };

    record Person(string Name, int Age);

    static string Describe(Person person) => person switch
    {
        { Age: < 18 } => "Minor",
        { Age: >= 18 and < 65 } => "Adult",
        { Age: >= 65 } => "Senior",
        _ => "Invalid age"
    };


    static bool DrawCircle()
    {
        Console.WriteLine("Drawing a circle...");
        // Circle drawing logic here
        return true;
    }

    static bool DrawSquare()
    {
        Console.WriteLine("Drawing a square...");
        // Square drawing logic here
        return true;
    }

    static bool DrawTriangle()
    {
        Console.WriteLine("Drawing a triangle...");
        // Triangle drawing logic here
        return true;
    }

    static bool Unknown()
    {
        Console.WriteLine("Unknown shape.");
        return false;
    }
}
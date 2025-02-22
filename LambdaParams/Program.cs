/*
 * This program demonstrates advanced usage of lambda expressions in C# with params arrays
 * and different collection types. It shows how to combine lambda expressions with 
 * flexible parameter arrays and LINQ operations.
 * 
 * Key concepts demonstrated:
 * 1. Lambda expressions with params array parameters
 * 2. Lambda expressions with different collection types (arrays, List<T>, HashSet<T>)
 * 3. LINQ operations within lambda expressions
 * 4. Complex collection processing using lambda expressions
 * 5. Generic delegate types (Func<T,TResult>) with collection parameters
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each lambda expression performs one specific operation
 *    - Processing methods have single responsibility of executing operations
 *    - Collection operations are focused on specific transformations
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New operations can be added as new lambda expressions
 *    - Processing methods remain unchanged while supporting new operations
 *    - Collection processing can be extended without modifying existing code
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Different collection types can be processed consistently
 *    - Lambda expressions work with base types and derived types
 *    - Processing methods handle any compatible delegate type
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Lambda expressions define minimal required functionality
 *    - Generic constraints ensure only necessary operations are required
 *    - Collection processing uses focused interfaces (IEnumerable<T>)
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Methods depend on delegate type abstractions
 *    - Collection processing depends on IEnumerable<T> interface
 *    - Implementation details are passed as lambda expressions
 * 
 * Examples include:
 * - Basic numeric operations (Average, Max)
 * - Conditional processing (filtering even numbers)
 * - String operations (sorting, joining)
 * - Complex collection analysis (grouping by length)
 * - Null handling with null-coalescing operator
 */

using System;
using System.Collections.Generic;
using System.Linq;

public class LambdaExample
{
    // Method that takes a lambda expression with a params array
    public static void ProcessNumbers(Func<int[], double> operation, params int[] numbers)
    {
        double result = operation(numbers);
        Console.WriteLine($"Result: {result}");
    }

    // Method that takes a lambda expression with an IEnumerable<T>
    public static void ProcessStrings(Func<IEnumerable<string>, string> operation, IEnumerable<string> strings)
    {
        string result = operation(strings);
        Console.WriteLine($"Result: {result}");
    }

    public static void Main()
    {
        // Using lambda with params array
        ProcessNumbers(nums => nums.Average(), 1, 2, 3, 4, 5);
        ProcessNumbers(nums => nums.Max(), 10, 20, 30);

        // Using lambda with params array and more complex logic
        ProcessNumbers(nums =>
        {
            var evenNumbers = nums.Where(n => n % 2 == 0);
            return evenNumbers.Any() ? evenNumbers.Average() : 0;
        }, 1, 2, 3, 4, 5, 6);

        // Using lambda with List<T>
        List<string> fruits = new List<string> { "apple", "banana", "cherry", "date" };
        ProcessStrings(strs => string.Join(", ", strs.OrderBy(s => s)), fruits);

        // Using lambda with array
        string[] colors = { "red", "green", "blue", "yellow" };
        ProcessStrings(strs => strs.FirstOrDefault(s => s.Length > 4) ?? "Not found", colors);

        // Using lambda with more complex collection processing
        ProcessStrings(strs =>
        {
            var groupedByLength = strs.GroupBy(s => s.Length);
            var mostCommonLength = groupedByLength.OrderByDescending(g => g.Count()).First().Key;
            return $"Most common length: {mostCommonLength}, Words: {string.Join(", ", groupedByLength.First(g => g.Key == mostCommonLength))}";
        }, new HashSet<string> { "cat", "dog", "elephant", "lion", "tiger", "bear" });
    }
}
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
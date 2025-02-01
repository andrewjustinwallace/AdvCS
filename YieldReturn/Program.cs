/*
This example demonstrates several common use cases for yield return outside of testing:

Generating Sequences: The Fibonacci method generates a Fibonacci sequence without storing all numbers in memory at once.
Filtering Collections: The GetEvenNumbers method filters a collection to return only even numbers, processing items one at a time.
Implementing Custom Collections: The Deck class implements IEnumerable<string> to create a custom collection of playing cards, generating each card on demand.
Lazy Loading of Data: The GetLargeDataSet method simulates loading a large dataset, but only processes items as they're requested.

SOLID Principles demonstrated:
1. Single Responsibility Principle (SRP):
   - Each iterator method has a single purpose
   - Deck class focuses solely on card enumeration
   - Filtering methods handle specific filtering logic
   - Data generation methods focus on specific sequences
 
2. Open/Closed Principle (OCP):
   - New sequence generators can be added without modifying existing ones
   - Filtering logic can be extended with new methods
   - Deck enumeration can be modified through inheritance
   - Data loading can be extended without changing consumers
 
3. Liskov Substitution Principle (LSP):
   - All iterators follow IEnumerable contract
   - Generic and non-generic enumerators are compatible
   - Iterator implementations maintain sequence behavior
   - Collection implementations preserve enumeration semantics
 
4. Interface Segregation Principle (ISP):
   - Methods implement only necessary iteration logic
   - Deck implements minimal required interfaces
   - Consumers depend only on IEnumerable
   - Iterator methods expose focused functionality
 
5. Dependency Inversion Principle (DIP):
   - Methods depend on IEnumerable abstractions
   - Consumers work with iterator interfaces
   - Implementation details are hidden from consumers
   - Enumeration logic is decoupled from usage

Key benefits of using yield return in these scenarios:

Memory Efficiency: It allows you to work with large sequences of data without loading everything into memory at once.
Improved Performance: For large datasets, it can be faster because it doesn't require creating and populating a full collection upfront.
Simplicity: It simplifies the implementation of iterators, making the code more readable and maintainable.
Lazy Evaluation: It enables lazy evaluation, where items are only generated when they're actually needed.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Example 1: Generating a sequence of numbers
        Console.WriteLine("Fibonacci Sequence:");
        foreach (var num in Fibonacci(10))
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine("\n");

        // Example 2: Filtering a collection
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        Console.WriteLine("Even numbers:");
        foreach (var num in GetEvenNumbers(numbers))
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine("\n");

        // Example 3: Implementing a custom collection
        var deck = new Deck();
        Console.WriteLine("Cards in the deck:");

        // Uses the generic GetEnumerator
        foreach (var card in deck)
        {
            Console.WriteLine(card);
        }
        Console.WriteLine();

        // This would use the non-generic GetEnumerator
        IEnumerable nonGenericDeck = deck;
        foreach (object card in nonGenericDeck)
        {
            Console.WriteLine(card);
        }

        // Example 4: Lazy loading of data
        Console.WriteLine("Lazy loading of large data set:");
        var lazyData = GetLargeDataSet().Take(5); // Only load the first 5 items
        foreach (var item in lazyData)
        {
            Console.WriteLine(item);
        }
    }

    // Example 1: Generating Fibonacci sequence
    static IEnumerable<int> Fibonacci(int count)
    {
        int current = 0, next = 1;
        for (int i = 0; i < count; i++)
        {
            yield return current;
            int temp = current;
            current = next;
            next = temp + next;
        }
    }

    // Example 2: Filtering a collection
    static IEnumerable<int> GetEvenNumbers(IEnumerable<int> numbers)
    {
        foreach (var num in numbers)
        {
            if (num % 2 == 0)
                yield return num;
        }
    }

    // Example 3: Implementing a custom collection
    public class Deck : IEnumerable<string>
    {
        private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        private static readonly string[] Ranks = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var suit in Suits)
            {
                foreach (var rank in Ranks)
                {
                    yield return $"{rank} of {suit}";
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // non-generic returns the generic
            return GetEnumerator();
        }
    }

    // Example 4: Lazy loading of data
    static IEnumerable<string> GetLargeDataSet()
    {
        Console.WriteLine("Starting to load data...");
        for (int i = 0; i < 1000000; i++)
        {
            if (i % 100000 == 0)
                Console.WriteLine($"Loaded {i} items...");
            yield return $"Item {i}";
        }
    }
}
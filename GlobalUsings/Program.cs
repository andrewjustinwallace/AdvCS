/*
 * This program demonstrates the use of global using directives in C# 10 and later.
 * Global usings allow you to specify using directives once for the entire project,
 * reducing code duplication and improving maintainability.
 * 
 * Key concepts demonstrated:
 * 1. Global using directives defined in GlobalUsings.cs
 * 2. Using aliases with global directives
 * 3. Project-wide namespace imports
 * 4. Built-in global usings in .NET 6+ projects
 * 
 * Benefits shown:
 * - Reduced boilerplate code
 * - Centralized namespace management
 * - Improved code organization
 * - Consistent namespace availability across project
 */

namespace GlobalUsingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // We can use List<T> without explicitly importing System.Collections.Generic
            List<string> fruits = new List<string> { "Apple", "Banana", "Cherry" };

            // We can use LINQ methods without explicitly importing System.Linq
            var sortedFruits = fruits.OrderBy(f => f).ToList();

            // We can use Console methods directly
            Console.WriteLine("Sorted fruits:");

            // We can also use the aliased ConsoleWriter
            foreach (var fruit in sortedFruits)
            {
                ConsoleWriter.WriteLine(fruit);
            }

            // We can use Task without explicitly importing System.Threading.Tasks
            Task.Delay(1000).Wait();
            Console.WriteLine("Done!");
        }
    }
}
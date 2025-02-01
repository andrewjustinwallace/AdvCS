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
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Global usings file has single responsibility of managing namespace imports
 *    - Each namespace provides focused, related functionality
 *    - Program class focuses on demonstrating usage without import concerns
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New namespaces can be added without modifying existing code
 *    - Aliases can be added without affecting existing code
 *    - Global using statements can be extended without changing consumer code
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Aliased types maintain substitutability with their base types
 *    - Global using statements preserve type relationships
 *    - Namespace hierarchies remain consistent
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Each global using imports only necessary namespaces
 *    - Aliases provide focused access to specific functionality
 *    - Projects only include required namespace imports
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Global usings can reference abstractions instead of concrete types
 *    - Aliases can map to interfaces or abstract classes
 *    - Dependencies are centrally managed at project level
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
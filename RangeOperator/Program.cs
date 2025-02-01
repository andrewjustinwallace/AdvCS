/*
 * This program demonstrates the Range operator (..) in C#, introduced in C# 8.0.
 * The Range operator provides a concise syntax for specifying a range of indices
 * in arrays and other collection types.
 * 
 * Key concepts demonstrated:
 * 1. Basic range syntax (x..y)
 * 2. Ranges with open ends (..y or x..)
 * 3. Using ranges with arrays
 * 4. Using ranges with List<T>
 * 5. Variable-based ranges
 * 6. List manipulation with ranges
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Each range operation performs a single, well-defined slice operation
 *    - Collection manipulations are focused on specific range operations
 *    - Example code segments demonstrate single concepts
 * 
 * 2. Open/Closed Principle (OCP):
 *    - Range operations work with any collection implementing IEnumerable
 *    - New range functionality can be added without modifying existing code
 *    - Collections can be extended to support custom range behavior
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - Range operations work consistently across different collection types
 *    - Arrays and Lists behave predictably with range operations
 *    - Range results maintain collection type contracts
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Range operations use minimal required collection interfaces
 *    - Collections only need to implement necessary slicing behavior
 *    - Clients only depend on the range operations they use
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Range operations work with collection abstractions
 *    - Code depends on collection interfaces rather than concrete types
 *    - Range functionality is decoupled from specific implementations
 * 
 * Examples show:
 * - Inclusive start, exclusive end ranges
 * - Start-only and end-only ranges
 * - Range operations on both arrays and lists
 * - Range variables for dynamic range selection
 * - List modifications using ranges
 */

int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

// Iterate over a range from index 2 to 6 (exclusive)
Console.WriteLine("Iterate over range 2..6:");
foreach (int num in numbers[2..6])
{
    Console.Write($"{num} ");
}
Console.WriteLine();

// Iterate from the beginning to index 4 (exclusive)
Console.WriteLine("Iterate over range ..4:");
foreach (int num in numbers[..4])
{
    Console.Write($"{num} ");
}
Console.WriteLine();

// Iterate from index 7 to the end
Console.WriteLine("Iterate over range 7..:");
foreach (int num in numbers[7..])
{
    Console.Write($"{num} ");
}
Console.WriteLine();

// Using variables to define the range
int start = 3;
int end = 8;
Console.WriteLine($"Iterate over range {start}..{end}:");
foreach (int num in numbers[start..end])
{
    Console.Write($"{num} ");
}
Console.WriteLine();


List<string> fruits = new List<string>
        {
            "Apple", "Banana", "Cherry", "Date", "Elderberry",
            "Fig", "Grape", "Honeydew"
        };

// Using range operator to get a sub-range
List<string> subList = fruits[2..5];
Console.WriteLine("Fruits from index 2 to 4:");
foreach (var fruit in subList)
{
    Console.WriteLine(fruit);
}

// Using range operator in a foreach loop
Console.WriteLine("\nFruits from index 3 to the end:");
foreach (var fruit in fruits[3..])
{
    Console.WriteLine(fruit);
}

// Using range operator with variables
start = 1;
end = fruits.Count - 1;
Console.WriteLine($"\nFruits from index {start} to {end - 1}:");
foreach (var fruit in fruits[start..end])
{
    Console.WriteLine(fruit);
}

// Using range operator to replace a range in the list
fruits.RemoveRange(1, 3);
fruits.InsertRange(1, new List<string> { "Blackberry", "Blueberry" });
Console.WriteLine("\nList after replacing elements:");
foreach (var fruit in fruits)
{
    Console.WriteLine(fruit);
}
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


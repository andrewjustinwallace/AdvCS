
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
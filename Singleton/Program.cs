/*
 * This program demonstrates the Singleton design pattern in C#, ensuring a class
 * has only one instance and providing a global point of access to that instance.
 * The implementation is thread-safe using double-check locking.
 * 
 * Key concepts demonstrated:
 * 1. Private constructor to prevent direct instantiation
 * 2. Static instance holder
 * 3. Lazy initialization
 * 4. Thread-safe implementation using lock
 * 5. Double-check locking pattern
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Singleton class manages its own instantiation
 *    - Instance management is encapsulated within the class
 *    - Count management is separate from instance management
 *    - Thread synchronization is handled internally
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New functionality can be added without modifying instance management
 *    - Thread safety mechanism can be extended without changing core logic
 *    - Additional features can be added through inheritance
 *    - Client code remains unchanged when singleton is modified
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All instances share the same behavior
 *    - Instance access remains consistent
 *    - Thread safety guarantees are maintained
 *    - Behavior is consistent across all access points
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Singleton exposes only necessary methods
 *    - Instance property provides focused access
 *    - Clients depend only on methods they use
 *    - Implementation details are hidden
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Singleton can implement interfaces for abstraction
 *    - Client code can depend on abstractions
 *    - Instance management is independent of usage
 *    - Implementation details are encapsulated
 * 
 * Pattern benefits:
 * - Ensures single instance exists
 * - Provides global access point
 * - Lazy initialization support
 * - Thread-safe operation
 * - Controls access to shared resource
 */

using System;
using System.Threading.Tasks;

namespace SingletonPatternDemo
{
    public sealed class Singleton
    {
        private static Singleton _instance = null;
        private static readonly object padlock = new object();
        private int _count = 0;

        private Singleton()
        {
            Console.WriteLine("Singleton instance created.");
        }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Singleton();
                        }
                    }
                }
                return _instance;
            }
        }

        public void IncrementCount()
        {
            _count++;
        }

        public void PrintCount()
        {
            Console.WriteLine($"Count: {_count}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Demonstrating Singleton Pattern");
            Console.WriteLine("===============================");

            // Using the singleton in a single-threaded context
            Singleton.Instance.IncrementCount();
            Singleton.Instance.IncrementCount();
            Singleton.Instance.PrintCount();

            // Using the singleton in a multi-threaded context
            Parallel.For(0, 5, _ =>
            {
                Singleton.Instance.IncrementCount();
            });

            Singleton.Instance.PrintCount();

            // Demonstrating that we always get the same instance
            Singleton instance1 = Singleton.Instance;
            Singleton instance2 = Singleton.Instance;

            Console.WriteLine($"Are both instances the same object? {ReferenceEquals(instance1, instance2)}");

            Console.ReadLine();
        }
    }
}
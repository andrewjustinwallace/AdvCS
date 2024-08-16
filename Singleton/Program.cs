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
/*
 * This program demonstrates the implementation of custom events in C# using the EventHandler<T> delegate pattern.
 * It implements a Counter class that raises an event when a specified threshold is reached.
 * 
 * Key concepts demonstrated:
 * 1. Custom EventArgs class (ThresholdReachedEventArgs) to pass event-specific data
 * 2. Standard event pattern implementation with EventHandler<T>
 * 3. Thread-safe event invocation using null-check pattern
 * 4. Protected virtual OnEventName method pattern for raising events
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Counter class handles only counting and threshold checking
 *    - ThresholdReachedEventArgs class only carries event data
 *    - Main program handles only user input and event subscription
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New event types can be added without modifying existing event handlers
 *    - Counter class can be extended through inheritance (virtual method)
 *    - Event handling can be extended without modifying Counter class
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - ThresholdReachedEventArgs extends EventArgs properly
 *    - Counter class can be inherited safely due to protected virtual method
 *    - Event handler delegates follow standard EventHandler<T> pattern
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - Event arguments contain only necessary properties
 *    - Counter exposes minimal public interface
 *    - Event subscription pattern follows standard .NET conventions
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Event handlers depend on EventArgs abstraction
 *    - Counter works with any compatible event handler
 *    - Components are loosely coupled through event pattern
 * 
 * Usage:
 * - Creates a Counter with a threshold of 111
 * - User presses 'a' to increment counter
 * - When threshold is reached, event is raised with threshold value and timestamp
 * - Program exits when threshold is reached
 */

using System;

namespace ConsoleApplication3
{
    class ProgramThree
    {
        static void Main(string[] args)
        {
            Counter c = new Counter(3);
            c.ThresholdReached += c_ThresholdReached;

            Console.WriteLine("press 'a' key to increase total");
            while (Console.ReadKey(true).KeyChar == 'a')
            {
                Console.WriteLine("adding one");
                c.Add(1);
            }
        }

        static void c_ThresholdReached(object sender, ThresholdReachedEventArgs e)
        {
            Console.WriteLine("The threshold of {0} was reached at {1}.", e.Threshold, e.TimeReached);
            Environment.Exit(0);
        }
    }

    class Counter
    {
        private int threshold;
        private int total;

        public Counter(int passedThreshold)
        {
            threshold = passedThreshold;
        }

        public void Add(int x)
        {
            total += x;
            if (total >= threshold)
            {
                ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                args.Threshold = threshold;
                args.TimeReached = DateTime.Now;
                OnThresholdReached(args);
            }
        }

        protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
        {
            EventHandler<ThresholdReachedEventArgs> handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
    }

    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Threshold { get; set; }
        public DateTime TimeReached { get; set; }
    }
}
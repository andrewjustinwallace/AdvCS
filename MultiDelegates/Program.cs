/*
 * This program demonstrates the use of multicast delegates in C#.
 * Multicast delegates allow multiple methods to be chained together
 * and executed in sequence when the delegate is invoked.
 * 
 * Key concepts demonstrated:
 * 1. Custom delegate definition (LogDel)
 * 2. Delegate instantiation with method references
 * 3. Delegate combination using the '+' operator
 * 4. Multicast delegate invocation
 * 5. Delegate parameter passing
 * 
 * SOLID Principles demonstrated:
 * 1. Single Responsibility Principle (SRP):
 *    - Log class handles only logging operations
 *    - Each log method has a single format responsibility
 *    - LogText method focuses solely on delegate invocation
 * 
 * 2. Open/Closed Principle (OCP):
 *    - New logging methods can be added without modifying existing code
 *    - Delegate chain can be extended without changing implementation
 *    - Log class can be extended with new formatting methods
 * 
 * 3. Liskov Substitution Principle (LSP):
 *    - All logging methods follow the same delegate signature
 *    - Methods can be substituted in the delegate chain
 *    - Behavior remains consistent with delegate contract
 * 
 * 4. Interface Segregation Principle (ISP):
 *    - LogDel defines minimal required method signature
 *    - Logging methods implement only necessary functionality
 *    - Clients depend only on the logging methods they need
 * 
 * 5. Dependency Inversion Principle (DIP):
 *    - Program depends on LogDel abstraction
 *    - Concrete logging methods are passed via delegate
 *    - Implementation details are decoupled through delegation
 * 
 * The example shows:
 * - Creating multiple delegate instances
 * - Combining delegates into a multicast delegate
 * - Passing delegates as method parameters
 * - Different logging format methods being called through the same delegate chain
 */

using System;

namespace AdvCS
{
    internal class Program
    {
        delegate void LogDel(string text);

        static void Main(string[] args)
        {
            var log = new Log();
            LogDel logDel, logDel2;

            logDel = new LogDel(log.LogTextToScreen);
            logDel2 = new LogDel(log.LogTextToScreen2);

            LogDel mLogDel = logDel + logDel2;

            Console.WriteLine("enter name:");

            var name = Console.ReadLine();

            LogText(mLogDel, name);

            Console.ReadKey();
        }

        static void LogText(LogDel logDel, string text)
        {
            logDel(text);
        }
    }

    public class Log
    {
        public void LogTextToScreen(string text)
        {
            Console.WriteLine($"-- {text}");
        }

        public void LogTextToScreen2(string text)
        {
            Console.WriteLine($"--- {text}");
        }
    }
}
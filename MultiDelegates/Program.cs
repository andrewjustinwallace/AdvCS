using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

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

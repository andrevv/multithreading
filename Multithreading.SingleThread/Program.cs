using System;
using System.Diagnostics;
using Multithreading.Utils;

namespace Multithreading.SingleThread
{
    internal static class Program
    {
        private static void Main()
        {
            WarmUp();

            const int size = 100;
            var data = Algorithms.GenerateRandomArray(size);

            var total = 0M;

            var sw = Stopwatch.StartNew();
            foreach (var e in data)
            {
                total += Algorithms.Factorial(e);
            }
            
            sw.Stop();

            Console.WriteLine($"Calculated {total} in {sw.Elapsed}.");
        }

        private static void WarmUp()
        {
            Algorithms.Factorial(5);
        }
    }
}
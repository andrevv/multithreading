using System;
using System.Diagnostics;
using Multithreading.SingleThread;
using Multithreading.MultipleThreads;

namespace Multithreading.Utils.Runner
{
    internal static class Program
    {
        private static void Main()
        {
            const int size = 200_000_000;
            var data = Algorithms.GenerateRandomArray(size);
        }

        private static void Measure(Func<int[], decimal> action, int[] data)
        {
            var sw = Stopwatch.StartNew();

            var result = action(data);
            
            sw.Stop();

            Console.WriteLine($"Calculated {result} in {sw.Elapsed}.");
        }
    }
}
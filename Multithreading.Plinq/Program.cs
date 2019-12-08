using System;
using System.Diagnostics;
using System.Linq;
using Multithreading.Utils;

namespace Multithreading.Plinq
{
    internal static class Program
    {
        private static void Main()
        {
            WarmUp();

            var data = Algorithms.GenerateRandomArray(size: 200_000_000);

            SumOfFactorials(data);
            ParallelSumOfFactorials(data);
        }
		
        private static void SumOfFactorials(int[] data)
        {
            var sw = Stopwatch.StartNew();

            var total = data.Sum(Algorithms.Factorial);

            sw.Stop();

            Console.WriteLine($"Calculated {total} in {sw.Elapsed}.");
        }
		
        private static void ParallelSumOfFactorials(int[] data)
        {
            var sw = Stopwatch.StartNew();

            var total = data.AsParallel().Sum(Algorithms.Factorial);

            sw.Stop();

            Console.WriteLine($"Calculated {total} in {sw.Elapsed}.");
        }
		
        private static void WarmUp()
        {
            var data = Algorithms.GenerateRandomArray(1000);
            SumOfFactorials(data);
            ParallelSumOfFactorials(data);
            Console.Clear();
        }
    }
}
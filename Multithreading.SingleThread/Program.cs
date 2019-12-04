using System;
using System.Diagnostics;

namespace Multithreading.SingleThread
{
    internal static class Program
    {
        private static void Main()
        {
            WarmUp();

            const int size = 200_000_000;

            var data = Generate(size);

            var total = 0M;

            var sw = Stopwatch.StartNew();
            foreach (var e in data)
            {
                total += Factorial(e);
            }
            
            sw.Stop();

            Console.WriteLine($"Calculated {total} in {sw.Elapsed}.");
        }

        private static decimal Factorial(int n)
        {
            var result = 1;
            for (var i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        private static int[] Generate(int size)
        {
            var data = new int[size];

            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < size; i++)
            {
                data[i] = random.Next(1, 10);
            }

            return data;
        }

        private static void WarmUp()
        {
            Factorial(5);
        }
    }
}
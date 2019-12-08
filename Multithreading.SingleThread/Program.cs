using System;
using System.Diagnostics;
using Multithreading.Utils;

namespace Multithreading.SingleThread
{
    public static class Program
    {
        private static void Main()
        {
            WarmUp();

            const int size = 100;
            var data = Algorithms.GenerateRandomArray(size);

            Run(data);
        }

        public static void Run(int[] data)
        {
        }

        private static void WarmUp()
        {
            Algorithms.Factorial(5);
        }
    }

    public static class ThreadingExtensions
    {
        public static decimal SingleThread(int[] data)
        {
            var total = 0M;
            foreach (var e in data)
            {
                total += Algorithms.Factorial(e);
            }

            return total;
        }
    }
}
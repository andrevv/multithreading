using System;
using System.Threading;
using Multithreading.Utils;

namespace Multithreading.MultipleThreads
{
    internal static class Program
    {
        private static void Main()
        {
            const int size = 100;
            var data = Algorithms.GenerateRandomArray(size);
            
            const int numberOfThreads = 1;
            
            var threads = new Thread[Math.Max(numberOfThreads - 1, 0)];
            var results = new decimal[Math.Max(numberOfThreads - 1, 0)];
            
            // Aggregated result
            var total = 0M;
            
            // Size of chunk to process in a thread
            var stride = (int) Math.Round(size / (double)numberOfThreads);

            // Process (numberOfThreads - 1) chunks in separate threads
            for (var i = 0; i < threads.Length; i++)
            {
                var t = i;
                var m = t * stride;
                var n = m + stride;
                threads[i] = new Thread(() =>
                {
                    var result = 0M;
                    for (var j = m; j < n; j++)
                    {
                        result += Algorithms.Factorial(data[j]);
                    }

                    results[t] = result;
                });
            }

            for (var i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }

            // Process the last chunk in the current thread
            for (var i = (numberOfThreads - 1) * stride; i < size; i++)
            {
                total += data[i];
            }

            for (var i = 0; i < threads.Length; i++)
            {
                // Wait a thread to complete
                threads[i].Join();

                // Aggregate the result
                total += results[i];
            }

            Console.WriteLine(total);
        }
    }
}
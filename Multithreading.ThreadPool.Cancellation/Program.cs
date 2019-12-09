using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Multithreading.Utils;

namespace Multithreading.ThreadPool.Cancellation
{
    internal static class Program
    {
        private static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();

        static Program()
        {
            Task.Run(() =>
            {
                Console.ReadLine();
                Console.WriteLine("Cancelling...");
                TokenSource.Cancel();
            });
        }

        private static void Main()
        {
            WarmUp();

            var data = Algorithms.GenerateRandomArray(200_000_000);

            var cancellationToken = TokenSource.Token;

            SumOfFactorials(data, 2, cancellationToken);
            SumOfFactorials(data, 4, cancellationToken);
            SumOfFactorials(data, 8, cancellationToken);
        }

        private static void SumOfFactorials(int[] data)
        {
            var sw = Stopwatch.StartNew();

            var total = data.Sum(Algorithms.Factorial);

            Console.WriteLine($"Calculated {total} in {sw.Elapsed} (single thread).");
        }

        private static void SumOfFactorials(int[] data, int numberOfTasks, CancellationToken cancellationToken)
        {
            var tasks = new List<Task<decimal>>();

            var chunkSize = data.Length / numberOfTasks;

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < numberOfTasks - 1; i++)
            {
                var start = chunkSize * i;
                var end = start + chunkSize;

                var task = Task.Run(() =>
                {
                    decimal sum = 0;
                    for (var j = start; j < end; j++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        sum += Algorithms.Factorial(data[j]);
                    }

                    return sum;
                }, cancellationToken);
                
                tasks.Add(task);
            }

            var lastTask = Task.Run(() =>
            {
                decimal sum = 0;
                for (var i = chunkSize * (numberOfTasks - 1); i < data.Length; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    sum += Algorithms.Factorial(data[i]);
                }

                return sum;
            }, cancellationToken);
            
            tasks.Add(lastTask);

            try
            {
                var total = tasks.Select(x => x.Result).Sum();

                Console.WriteLine($"Calculated {total} in {sw.Elapsed} ({numberOfTasks} tasks).");
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle(exception =>
                {
                    if (exception is TaskCanceledException)
                    {
                        // Task was cancelled.
                        Console.WriteLine("Operation was cancelled.");
                        return true;
                    }

                    Console.WriteLine($"Error occured: {exception.Message}.");
                    
                    TokenSource.Cancel();

                    return true;
                });
            }
        }

        private static void WarmUp()
        {
            var data = Algorithms.GenerateRandomArray(1000);
            SumOfFactorials(data);
            SumOfFactorials(data, 4, CancellationToken.None);
            Console.Clear();
        }
    }
}
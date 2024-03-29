﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Multithreading.Utils;

namespace Multithreading.ThreadPool
{
    internal static class Program
    {
        private static void Main()
        {
            WarmUp();

            var data = Algorithms.GenerateRandomArray(200_000_000);

            SumOfFactorials(data);
            SumOfFactorials(data, 2);
            SumOfFactorials(data, 4);
            SumOfFactorials(data, 8);
            SumOfFactorials(data, 10);
            SumOfFactorials(data, 20);
        }

        private static void SumOfFactorials(int[] data)
        {
            var sw = Stopwatch.StartNew();

            var total = data.Sum(Algorithms.Factorial);

            Console.WriteLine($"Calculated {total} in {sw.Elapsed} (single thread).");
        }

        private static void SumOfFactorials(int[] data, int numberOfTasks)
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
                        sum += Algorithms.Factorial(data[j]);
                    }

                    return sum;
                });
                
                tasks.Add(task);
            }

            var lastTask = Task.Run(() =>
            {
                decimal sum = 0;
                for (var i = chunkSize * (numberOfTasks - 1); i < data.Length; i++)
                {
                    sum += Algorithms.Factorial(data[i]);
                }

                return sum;
            });
            
            tasks.Add(lastTask);

            var total = tasks.Select(x => x.Result).Sum();

            Console.WriteLine($"Calculated {total} in {sw.Elapsed} ({numberOfTasks} tasks).");
        }

        private static void WarmUp()
        {
            var data = Algorithms.GenerateRandomArray(1000);
            SumOfFactorials(data);
            SumOfFactorials(data, 4);
            Console.Clear();
        }
    }
}
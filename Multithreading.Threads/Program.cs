using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Multithreading.Utils;

namespace Multithreading.Threads
{
	public static class Program
	{
		private static void Main()
		{
			WarmUp();

			var data = Algorithms.GenerateRandomArray(size: 200_000_000);

			SumOfFactorials(data);
			SumOfFactorials(data, numberOfThreads: 2);
			SumOfFactorials(data, numberOfThreads: 4);
			SumOfFactorials(data, numberOfThreads: 8);
			SumOfFactorials(data, numberOfThreads: 10);
			SumOfFactorials(data, numberOfThreads: 20);
		}

		private static void SumOfFactorials(int[] data)
		{
			var sw = Stopwatch.StartNew();

			var sum = data.Sum(Algorithms.Factorial);

			sw.Stop();

			Console.WriteLine($"Calculated {sum} in {sw.Elapsed}.");
		}

		private static void SumOfFactorials(int[] data, int numberOfThreads)
		{
			var threads = new Thread[numberOfThreads - 1];
			var results = new decimal[numberOfThreads - 1];

			var chunkSize = data.Length / numberOfThreads;

			for (var i = 0; i < numberOfThreads - 1; i++)
			{
				var threadIndex = i;
				var start = chunkSize * i;
				var end = start + chunkSize;

				threads[i] = new Thread(() =>
				{
					decimal sum = 0;
					for (var j = start; j < end; j++)
					{
						sum += Algorithms.Factorial(data[j]);
					}

					results[threadIndex] = sum;
				});
			}

			var sw = Stopwatch.StartNew();

			for (var i = 0; i < numberOfThreads - 1; i++)
			{
				threads[i].Start();
			}

			decimal total = 0;
			for (var i = chunkSize * (numberOfThreads - 1); i < data.Length; i++)
			{
				total += Algorithms.Factorial(data[i]);
			}

			for (var i = 0; i < numberOfThreads - 1; i++)
			{
				threads[i].Join();
				total += results[i];
			}

			Console.WriteLine($"Calculated {total} in {sw.Elapsed} in {numberOfThreads} threads.");
		}

		private static void WarmUp()
		{
			var data = Algorithms.GenerateRandomArray(1000);
			SumOfFactorials(data);
			SumOfFactorials(data, 2);
			Console.Clear();
		}
	}
}
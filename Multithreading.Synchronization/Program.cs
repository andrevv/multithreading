using System;
using System.Threading;

namespace Multithreading.Synchronization
{
    static class Program
    {
        private static void Main()
        {
            UnSynchronized();
            Synchronized();
        }

        private static void UnSynchronized()
        {
            var counter = 0;

            Console.WriteLine($"UnSynchronized before: {counter}.");

            const int numberOfThreads = 10;
            const int numberOfIterations = 10_000;

            var threads = new Thread[numberOfThreads];
            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (var j = 0; j < numberOfIterations; j++)
                    {
                        counter++;
                        Thread.SpinWait(100);
                        counter--;
                    }
                });
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine($"UnSynchronized after: {counter}.");
        }

        private static void Synchronized()
        {
            var counter = 0;
            var counterLock = new object();

            Console.WriteLine($"Synchronized before: {counter}.");
            
            const int numberOfThreads = 10;
            const int numberOfIterations = 1_000_000;

            var threads = new Thread[numberOfThreads];
            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (var j = 0; j < numberOfIterations; j++)
                    {
                        lock (counterLock)
                        {
                            counter++;
                        }

                        Thread.SpinWait(100);

                        lock (counterLock)
                        {
                            counter--;
                        }
                    }
                });
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine($"Synchronized after: {counter}.");
        }
    }
}
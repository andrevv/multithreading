using System;
using System.Diagnostics;
using System.Threading;

namespace Multithreading.Synchronization
{
    static class Program
    {
        private static void Main()
        {
            const int numberOfThreads = 10;
            const int numberOfCycles = 1_000_000;
            
            UnSynchronized(numberOfThreads, numberOfCycles);
            
            //Locked(numberOfThreads, numberOfCycles);
            
            //Atomic(numberOfThreads, numberOfCycles);
        }

        private static void UnSynchronized(int numberOfThreads, int numberOfCycles)
        {
            var counter = 0;

            var threads = new Thread[numberOfThreads];
            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (var j = 0; j < numberOfCycles; j++)
                    {
                        counter++;
                        Thread.SpinWait(100);
                        counter--;
                    }
                });
            }

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }
            
            sw.Stop();

            Console.WriteLine($"Counter: {counter}. Ran in {sw.Elapsed}.");
        }

        private static void Locked(int numberOfThreads, int numberOfCycles)
        {
            var counter = 0;
            var counterLock = new object();

            var threads = new Thread[numberOfThreads];
            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (var j = 0; j < numberOfCycles; j++)
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

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }

            sw.Stop();

            Console.WriteLine($"Counter: {counter}. Ran in {sw.Elapsed}.");
        }

        private static void Atomic(int numberOfThreads, int numberOfCycles)
        {
            var counter = 0;
            
            var threads = new Thread[numberOfThreads];
            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (var j = 0; j < numberOfCycles; j++)
                    {
                        Interlocked.Increment(ref counter);
                        Thread.SpinWait(100);
                        Interlocked.Decrement(ref counter);
                    }
                });
            }

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Start();
            }

            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }
            
            sw.Stop();

            Console.WriteLine($"Counter: {counter}. Ran in ${sw.Elapsed}.");
        }
    }
}
using System;
using System.Threading;

namespace Multithreading.Synchronization.Atomics
{
    internal static class Program
    {
        private static void Main()
        {
            var acquired = 0;
            var counter = 0;

            const int numberOfThreads = 10;

            var threads = new Thread[numberOfThreads];
            for (var i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(() =>
                {
                    while (true)
                    {
                        if (Interlocked.Exchange(ref acquired, 1) == 0)
                        {
                            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} acquired the lock.");
                            
                            Thread.Sleep(1000);

                            counter += 1;

                            Interlocked.Exchange(ref acquired, 0);
                            
                            break;
                        }
                        else
                        {
                            Thread.SpinWait(100);
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

            Console.WriteLine($"Counter: {counter}.");
        }
    }
}

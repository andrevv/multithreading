using System;
using System.Threading;

namespace Multithreading.Synchronization.ManualResetEvents
{
    internal static class Program
    {
        private static void Main()
        {
            var mre = new ManualResetEvent(initialState: false);

            const int numberOfThreads = 3;
            
            for (var i = 0; i < numberOfThreads; i++)
            {
                var thread = new Thread(threadIndex =>
                {
                    for (var j = 0; j < 100; j++)
                    {
                        mre.WaitOne();

                        Console.WriteLine($"Thread {threadIndex} is released.");

                        Thread.Sleep(1500); 
                    }
                });

                thread.Start(i);
            }

            for (var i = 0; i < 5; i++)
            {
                Console.ReadLine();

                if (i % 2 == 0)
                {
                    Console.WriteLine("ManualResetEvent is switched to signaled state.");
                    mre.Set();
                }
                else
                {
                    Console.WriteLine("ManualResetEvent is switched to non-signaled state.");
                    mre.Reset();
                }
            }
        }
    }
}
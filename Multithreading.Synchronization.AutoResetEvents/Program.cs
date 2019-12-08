using System;
using System.Threading;

namespace Multithreading.Synchronization.AutoResetEvents
{
    internal static class Program
    {
        private static void Main()
        {
            var are = new AutoResetEvent(initialState: false);

            const int numberOfThreads = 10;
            
            for (var i = 0; i < numberOfThreads; i++)
            {
                var thread = new Thread(threadIndex =>
                {
                    are.WaitOne();

                    Console.WriteLine($"Thread {threadIndex} is released.");
                });

                thread.Start(i);
            }

            for (var i = 0; i < 10; i++)
            {
                Console.ReadLine();

                are.Set();
            }
        }
    }
}
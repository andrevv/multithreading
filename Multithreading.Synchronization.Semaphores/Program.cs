using System;
using System.Threading;

namespace Multithreading.Synchronization.Semaphores
{
    internal static class Program
    {
        private static void Main()
        {
            var semaphore = new Semaphore(0, 3);

            const int numberOfThreads = 50;

            for (var i = 0; i < numberOfThreads; i++)
            {
                var thread = new Thread(threadIndex =>
                {
                    semaphore.WaitOne();
					
                    Console.WriteLine($"Thread {threadIndex} enters the semaphore.");

                    Thread.Sleep(3000);

                    semaphore.Release();
                });

                thread.Start(i);
            }

            Console.WriteLine("Threads started.");
			
            semaphore.Release(3);
        }
    }
}
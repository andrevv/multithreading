using System;
using System.Threading;

namespace Multithreading.Synchronization.Deadlock
{
    internal static class Program
    {
        private static void Main()
        {
            var a = new object();
            var b = new object();

            var threadA = new Thread(() =>
            {
                lock (a)
                {
                    Console.WriteLine("Thread A acquired the 'a' lock.");
                    
                    Thread.Sleep(1000);
                    lock (b)
                    {
                        Console.WriteLine("Thread A acquired the 'b' lock.");
                    }
                }
            });

            var threadB = new Thread(() =>
            {
                lock (b)
                {
                    Console.WriteLine("Thread B acquired the 'b' lock.");
                    
                    Thread.Sleep(1000);
                    lock (a)
                    {
                        Console.WriteLine("Thread B acquired the 'a' lock.");
                    }
                }
            });

            threadA.Start();
            threadB.Start();
            threadA.Join();
            threadB.Join();

            Console.WriteLine("Done");
        }
    }
}
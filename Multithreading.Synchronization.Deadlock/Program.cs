using System;
using System.Threading;

namespace Multithreading.Synchronization.Deadlock
{
    internal static class Program
    {
        private static void Main()
        {
//            Deadlock();
            DeadlockPrevention();
        }

        private static void Deadlock()
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

        private static void DeadlockPrevention()
        {
            var a = new object();
            var b = new object();

            var threadA = new Thread(() =>
            {
                lock (a)
                {
                    Console.WriteLine("Thread A acquired the 'a' lock.");

                    lock (b)
                    {
                        Console.WriteLine("Thread A acquired the 'b' lock.");
                        
                        //
                        // Do work
                        //
                        
                        Thread.Sleep(5000);
                    }
                }
            });

            var threadB = new Thread(() =>
            {
                var acquiredA = false;
                var acquiredB = false;
                var timeout = TimeSpan.FromSeconds(1);
                const int maxRetries = 10;

                for (var retry = 0; retry < maxRetries; retry++)
                {
                    try
                    {
                        acquiredB = Monitor.TryEnter(b, timeout);
                        if (acquiredB)
                        {
                            Console.WriteLine("Thread B acquired the 'b' lock.");

                            try
                            {
                                acquiredA = Monitor.TryEnter(a, timeout);
                                if (acquiredA)
                                {
                                    Console.WriteLine("Thread B acquired the 'a' lock.");

                                    //
                                    // Do work
                                    //
                                    
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Thread B did not acquire the 'a' lock.");
                                }
                            }
                            finally
                            {
                                if (acquiredA)
                                {
                                    Monitor.Exit(a);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Thread B did not acquire the 'b' lock.");
                        }
                    }
                    finally
                    {
                        if (acquiredB)
                        {
                            Monitor.Exit(b);
                        }
                    }
                }

                Console.WriteLine("Thread B could not acquire locks.");
            });

            threadA.Start();
            threadB.Start();
            threadA.Join();
            threadB.Join();

            Console.WriteLine("Done");
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading.Synchronization.Volatiles
{
    internal class Program
    {
        private volatile bool _continue = true;

        private static void Main()
        {
            var p = new Program();
            
            Task.Run(() =>
            {
                p._continue = false;
            });
            
            while (p._continue)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Running.");
            }

            Console.WriteLine("Done.");
        }
    }
}
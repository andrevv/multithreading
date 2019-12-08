using System;

namespace Multithreading.Utils
{
    public static class Algorithms
    {
        public static decimal Factorial(int n)
        {
            var result = 1;
            for (var i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
        
        public static int[] GenerateSequentialArray(int size)
        {
            var data = new int[size];

            for (var i = 0; i < size; i++)
            {
                data[i] = i;
            }

            return data;
        }
        
        public static int[] GenerateRandomArray(int size)
        {
            const int min = 1;
            const int max = 20;

            var data = new int[size];

            var random = new Random(Environment.TickCount);
            for (var i = 0; i < size; i++)
            {
                data[i] = random.Next(min, max);
            }

            return data;
        }
    }
}
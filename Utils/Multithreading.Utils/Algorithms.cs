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
            var data = new int[size];

            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < size; i++)
            {
                data[i] = random.Next(1, 10);
            }

            return data;
        }
    }
}
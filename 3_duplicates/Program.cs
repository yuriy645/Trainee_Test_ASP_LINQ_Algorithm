using System;

namespace _3_duplicates
{//Write a program to remove duplicates from a sorted int[]
//INPUT: int[] {1,2,3,4,4,56}
//OUTPUT: int[] { 1,2,3,4,56}
//You are not allowed to modify an existing array.
//You are not allowed to use LINQ.

    class Program
    {
        static void Main(string[] args)
        {
            int[]inputArray = new int[] { 1, 2, 3, 4, 4, 56 };
            
            int[] outputArray = new int[inputArray.Length];

            
            outputArray[0] = inputArray[0];
            int n = 1;
            
            for (int i = 1; i < inputArray.Length; i++)  
            {
                if (inputArray[i] != inputArray[i-1])
                {
                    outputArray[n] = inputArray[i];
                    n++;
                }
            }

            int[] outputArray1 = new int[n];
            for (int i = 0; i < n; i++)
            {
                outputArray1[i] = outputArray[i];
            }

            Console.Write("Входной массив:   ");
            for (int i = 0; i < inputArray.Length; i++)  
            {
                Console.Write($"{inputArray[i]}  ");
            }

            Console.Write("\nВЫходной массив:  ");
            for (int i = 0; i < n ; i++)  
            {
                Console.Write($"{outputArray1[i]}  ");
            }
            Console.WriteLine();
            
        }
    }
}

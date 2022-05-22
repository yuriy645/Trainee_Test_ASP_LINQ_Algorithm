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
            int[] inputArray = new int[] { 1, 2, 3, 4, 4, 56 };

            int outLengthCount = 1;
            for (int i = 1; i < inputArray.Length; i++)// считаем заранее длину нового массива
            {
                if (inputArray[i] != inputArray[i - 1])
                {
                    outLengthCount++;
                }
            }
           
            int[] outputArray = new int[outLengthCount];

            outputArray[0] = inputArray[0];

            outLengthCount = 1;
            for (int i = 1; i < inputArray.Length; i++)
            {
                if (inputArray[i] != inputArray[i - 1])
                {
                    outputArray[outLengthCount] = inputArray[i];
                    outLengthCount++;
                }
            }


            Console.Write("Входной массив:   ");
            for (int i = 0; i < inputArray.Length; i++)
            {
                Console.Write($"{inputArray[i]}  ");
            }

            Console.Write("\nВЫходной массив:  ");
            for (int i = 0; i < outLengthCount; i++)
            {
                Console.Write($"{outputArray[i]}  ");
            }
            Console.WriteLine();

        }
    }
}

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2_LINQ
{//Group the unique words of same length in a given sentence, sort the groups in increasing order and display the groups,
 //the word count and the words of that length.
    class Program
    {
        static void Main()
        {
            string input = "The “C# Professional” course includes the topics I discuss in my CLR via C# book and ";
            input += "teaches how the CLR works thereby showing you how to develop applications and reusable components for the .NET Framework.";

            Console.WriteLine("Вывод входного текста:");
            Console.WriteLine(input + "\n");

            Console.WriteLine("Вывод списка обнаруженных слов:");

            //1-й способ, так не всегда можно отделить пробелы скраю
            //MatchCollection wordsCollection = new Regex(@"\s*[.]*[0-9A-Za-z#]+\s*").Matches(input);
            //foreach (Match element in wordsCollection)
            //{
            //    Console.Write("-{0}-", element.Value);
            //}

            //2-й способ, точное определение
            var wordsCollection = Regex.Matches(input, @"\s*“*(?<worrd>\S+)[”*.*\s*]")
                                       .Select(m => m.Groups["worrd"].Value)
                                       .Distinct();  //убираем дубликаты

            foreach (string element in wordsCollection)
            {
                Console.Write("-{0}-", element); //.Groups["worrd"]
            }
            Console.WriteLine("\n");

            //string[] words = { "The", "C#", "Professional", "course", "includes", "the", "topics", "I", "discuss", "in" };

            var query = from word in wordsCollection
                        orderby word.Length                      
                        group word by word.Length into partition 
                        select new { Key = partition.Key, Count = partition.Count(), Group = partition };


            foreach (var item in query)
            {
                Console.Write("Words of length: {0}, ", item.Key);
                Console.WriteLine("Count: {0}", item.Count);

                foreach (var wordElement in item.Group)
                    Console.WriteLine("{0} ", wordElement);

                //Console.WriteLine();
            }

            // Delay.
            Console.ReadKey();
        }
    }
}

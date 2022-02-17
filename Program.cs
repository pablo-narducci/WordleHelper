using System;
using System.Collections.Generic;
using System.Linq;
using WordleHelper.IO;
using WordleHelper.Solver;

namespace WordleHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = TryGetFilename(args);

            var words = WordsFileReader.ReadWords(filename);

            var greeting = filename != null ? $"that you provided me in the {filename} file!" : "in the English Wordle! \nIf you want to use a different list of words, create a file and give me the path to it as the first parameter";

            Console.WriteLine($"Let's try to win at Wordle. I'll be your infalible memory here.\n" +
                $"I've read and memorized all the words {greeting}.\n");

            while (words.Count > 1)
            {
                var guess = ReadUntilOk("Enter a 5 letter guess: ");
                var result = ReadUntilOk("Enter result 'o' for OK, 'm' for misplaced, 'n' for no: ",
                    ContainsOnlyValidEnumValues(typeof(Result)));

                IEnumerable<Rule> newRules = GetListOfRules(guess, result);

                words = words.Where(word => newRules.All(rule => rule.Matches(word))).ToList();

                Console.WriteLine($"There are {words.Count} words that match.");
                words.ForEach(x => Console.WriteLine(x));
                Console.WriteLine($"There are {words.Count} words that match.");
            }

            if (words.Count == 0)
            {
                Console.WriteLine("Too bad... no words match.");
            }
            else
            {
                var solution = words[0];
                Console.WriteLine($"We're done! The word is ¡¡ >>>{solution.ToString().ToUpper()}<<< !!.");
                Console.WriteLine("Do you know the definition of today's word? (y/n): ");
                var yn = Console.ReadLine();
                if ("n".Equals(yn))
                {
                    Console.WriteLine($"No problem! Here I have some definitions for {solution}:");
                    Console.WriteLine(DictionaryFactory.Create().GetDefinition(solution.ToString()));
                }
                else
                {
                    Console.WriteLine("Ok, no definitions today then!");
                }
            }

            Console.WriteLine("Press enter to leave");
            Console.ReadLine();
        }

        private static string TryGetFilename(string[] args)
        {
            string filename = null;

            if (args != null && args.Length > 0)
            {
                filename = args[0];
            }

            return filename;
        }

        private static Func<string, bool> ContainsOnlyValidEnumValues(Type enumType)
        {
            var vs = Enum.GetValues(enumType).Cast<int>().ToList().Select(x => (char)x).ToList();
            return x => x.All(y => vs.Contains(y));
        }

        private static string ReadUntilOk(string instructions, Func<string, bool> validation = null)
        {
            string input;
            do
            {

                Console.Write(instructions);
                input = Console.ReadLine();
            } while (input.Length != 5 || (validation != null && !validation(input)));

            return input.ToLower();
        }

        private static IEnumerable<Rule> GetListOfRules(string guess, string result)
        {
            if (guess.Length != 5 || result.Length != 5)
            {
                throw new Exception("Wrong lenght");
            }

            for(int i = 0; i < 5; i++)
            {
                yield return RuleFactory.Create(result[i], guess[i], i);
            }
        }
    }
}

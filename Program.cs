using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Wordler
{
    public class Globals
    {
        private static string[] _answers = File.ReadAllLines(@"/Users/christoffer/Programming/dotnet/Wordler/answers.txt");

        public static string[] Answers
        {
            get => _answers;
            set => _answers = value;
        }
        private static string[] _validWords = File.ReadAllLines(@"/Users/christoffer/Programming/dotnet/Wordler/valid_words.txt");

        public static string[] ValidWords
        {
            get => _validWords;
            set => _validWords = value;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            /*string[] letters = {"a", "b", "c", "d", "e", "f", "g","h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};*/
            var answerLetterCount = new Dictionary<string, int>{};
            Dictionary<string, int> validWordScore = new Dictionary<string, int>{};

            foreach (var s in Globals.Answers)
            {
                foreach (var c in s)//.Select(c => c).Distinct())
                {
                    answerLetterCount.TryGetValue(c.ToString(), out var currentCount);
                    answerLetterCount[c.ToString()] = currentCount + 1;
                }
            }

            var sortedValidWords = ScoreAnswers(answerLetterCount);

            PrintDict("\nFirst letter scores: ", answerLetterCount);


            var secondAnswerLetterCount = answerLetterCount
                    .ToDictionary(
                        e => e.Key, 
                        e => sortedValidWords.First().Key.Contains(e.Key) ? 0 : e.Value); 
            
            ScoreAnswers(secondAnswerLetterCount);
            
            PrintDict("\nSecond letter scores: ", secondAnswerLetterCount);

            Console.WriteLine("\nValid Word Count: {0,3}",Globals.ValidWords.Length);
            Console.WriteLine("Answers Count: {0,3}",Globals.Answers.Length);
        }

        static Dictionary<string, int> ScoreAnswers(Dictionary<string, int> pDict)
        {
            
            Dictionary<string, int> validWordScore = new Dictionary<string, int>{};
            foreach (var s in Globals.Answers)
            {
                int score = 0;
                foreach (var c in s.Select(c => c).Distinct())
                {
                    score += pDict[c.ToString()];
                }

                validWordScore[s] = score;
            }

            return SortAnswers(validWordScore);
        }

        static Dictionary<string, int> SortAnswers(Dictionary<string, int> pDict)
        {
            Console.WriteLine("Sorted List: ");
            var rDict = pDict.OrderByDescending(s => s.Value).Take(5).ToDictionary(s => s.Key, s=> s.Value);
            foreach (var s in rDict)
            {
                Console.WriteLine(s.Key + ": " + s.Value);
            }

            return rDict;
        }

        static void PrintDict(string pString, Dictionary<string, int> pDict)
        {
            Console.WriteLine(pString);
            foreach (var kvp in pDict)
            {
                Console.Write(kvp.Key + ": " + kvp.Value+", ");
            }
            Console.WriteLine("\n");
        }
    }
}

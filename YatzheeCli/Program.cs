using System;
using System.Collections.Generic;
using System.Linq;
using Yatzhee;

namespace YatzheeCli
{
    class Program
    {
        private static readonly Random Random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome! Would you like to play a game of Yatzhee?");
            
            Console.WriteLine("Hit <enter> to roll the dies and let the machine select which category to score the roll in.");
            Console.WriteLine("Type Ctrl-C or q to quit the game.");                
            
            try
            {
                var totalScore = GameLoop(Console.WriteLine, Console.ReadLine);
                Console.WriteLine("Game over! Your score is {0}.", totalScore);                
            }
            catch (Exception e)
            {
                Console.WriteLine("I suspect you gave me some instructions I do not understand. Restart to try again!");
            }

            Console.ReadLine();
        }

        private static int GameLoop(Action<string> output, Func<string> input)
        {
            var nextCommand = input();
            var availableCategories = Category.Categories.ToList();
            var totalScore = 0;
            while (!"q".Equals(nextCommand, StringComparison.OrdinalIgnoreCase) && availableCategories.Count > 0)
            {
                var rollDices = RollDices(5);
                var rollCount = 1;

                output($"Roll {rollCount}: {string.Join(",", rollDices)}");
                output("Reroll any dies? Enter the index of the dies you want to reroll (1-5).");
                
                do
                {
                    var reroll = input().Trim().ToCharArray().Where(char.IsNumber).ToList();
                    if (reroll.Any())
                    {
                        rollDices = Reroll(rollDices, reroll.Select(s => int.Parse(s.ToString()) - 1).ToArray());
                        rollCount++;
                        output($"Roll {rollCount}: {string.Join(",", rollDices)}");
                    }
                    else 
                    {
                        break;
                    }

                } while (rollCount < 3);                

                var bestScore = new YatzheeScorer(availableCategories.ToArray()).MaxScore(string.Join(",", rollDices));
                output($"Score: {bestScore.Item1} for {Category.CategoryName(bestScore.Item2)}");
                availableCategories.Remove(bestScore.Item2);
                totalScore += bestScore.Item1;
                output("Hit <enter> to roll again.");
                nextCommand = input();
            }
            return totalScore;
        }

        private static List<string> RollDices(int numDies)
        {
            return Enumerable.Range(1, numDies)
                    .Select(i => Random.Next(1, 7).ToString())
                    .ToList();
        }

        private static List<string> Reroll(List<string> roll, int[] rerollIndices)
        {
            var reroll = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (rerollIndices.Contains(i))
                    reroll.Add(RollDices(1).First());
                else
                    reroll.Add(roll[i]);
            }
            return reroll;
        }
    }
}

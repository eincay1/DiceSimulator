using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGameSimulator
{
    internal class Program
    {
        private const int NumSimulations = 10000;
        private const int NumDice = 5;
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var scoreCounts = SimulateGames();
            stopwatch.Stop();

            PrintResults(scoreCounts, stopwatch.Elapsed.TotalSeconds);
            // Keep console open 
            Console.WriteLine("\nPress Enter to exit.....");
            Console.ReadLine();
        }

        private static Dictionary<int, int> SimulateGames()
        {
            var scoreCounts = new Dictionary<int, int>();

            for (int i = 0; i < NumSimulations; i++)
            {
                int score = PlayGame();

                if (scoreCounts.ContainsKey(score))
                    scoreCounts[score]++;
                else
                    scoreCounts[score] = 1;
            }

            return scoreCounts;
        }

        private static int PlayGame()
        {
            List<int> dice = new List<int>(NumDice);
            int totalScore = 0;
            int diceCount = NumDice;

            while (diceCount > 0)
            {
                // Roll all remaining dices
                dice.Clear();
                for (int i = 0; i < diceCount; i++)
                {
                    dice.Add(random.Next(1, 7));
                }

                int rollScore = ProcessRoll(dice);
                totalScore += rollScore;

                // Determine how many dice to remove
                if (dice.Any(d => d == 3))
                {
                    // Remove all 3's
                    int threesCount = dice.Count(d => d == 3);
                    diceCount -= threesCount;
                }
                else
                {
                    // Remove lowest dice
                    diceCount--;
                }
            }

            return totalScore;
        }

        private static int ProcessRoll(List<int> dice)
        {
            if (dice.Any(d => d == 3))
            {
                return 0;
            }
            else
            {
                return dice.Min();
            }
        }

        private static void PrintResults(Dictionary<int, int> scoreCounts, double elapsedSeconds)
        {
            Console.WriteLine($"Number of simulations was {NumSimulations} usin {NumDice} dice.");

            // Get all possible scores ordered 
            var sortedScores = scoreCounts.Keys.OrderBy(score => score).ToList();

            foreach (int score in sortedScores)
            {
                int count = scoreCounts[score];
                double percentage = (double)count / NumSimulations;

                Console.WriteLine($"Total {score} occurs {percentage:F3} occurred {count}.0 times.");
            }

            Console.WriteLine($"Total simulation took {elapsedSeconds:F1}seconds.");
        }
    }
}

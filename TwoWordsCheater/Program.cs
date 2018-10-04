using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TODO: Bonus for 8 letter words?
 * TODO: Complex forms = only handling single line right now. What about parallel or overlapping words? What about replace a letter in original word to make new word?
 */

namespace TwoWordsCheater
{
	class Program
	{
		static List<string> _baseWordList = System.IO.File.ReadAllLines(
			@"C:\users\Jeremiah\Documents\repo\TwoWordsCheater\TwoWordsCheater\TwoWordsCheater\bin\Debug\sowpods.txt")
			.ToList();
		static Dictionary<char, int> _alphabetPointValues = new Dictionary<char, int>()
		{
			{'a', 1 },
			{'b', 3 },
			{'c', 3 },
			{'d', 1 },
			{'e', 1 },
			{'f', 3 },
			{'g', 2 },
			{'h', 3 },
			{'i', 1 },
			{'j', 5 },
			{'k', 4 },
			{'l', 3 },
			{'m', 2 },
			{'n', 1 },
			{'o', 1 },
			{'p', 2 },
			{'q', 6 },
			{'r', 1 },
			{'s', 1 },
			{'t', 1 },
			{'u', 2 },
			{'v', 4 },
			{'w', 3 },
			{'x', 6 },
			{'y', 3 },
			{'z', 6 }
		};
		static List<string> _permutationList = new List<string>();


		static void Main(string[] args)
		{
			var continuePlaying = true;

			while (continuePlaying)
			{
				Console.WriteLine("Enter the word in play.");
				var currentWord = Console.ReadLine();
				Console.WriteLine("Enter the letters available to you.");
				var availableLetters = Console.ReadLine();

				var possiblePlayList = new List<string>();

				//bAleen = 60
				// TODO: Eliminate the duplicate permutations when using the wildcard character
				possiblePlayList.AddRange(GetPossibleWords(currentWord, availableLetters));
				

				var highestScoringPlay = "";
				var highestScore = 0;

				foreach (var play in possiblePlayList)
				{
					var wordScore = CalculateScore(play);

					Console.WriteLine("Found {0} worth {1} point(s).", play, wordScore);

					if (wordScore > highestScore)
					{
						highestScore = wordScore;
						highestScoringPlay = play;
						Console.WriteLine("NEW HIGH SCORE!");
					}
				}

				Console.WriteLine("-----------------------");
				Console.WriteLine($"BEST PLAY: {highestScoringPlay}");
				Console.WriteLine($"WORTH: {highestScore}");
				Console.WriteLine("-----------------------");
				Console.WriteLine();
				Console.WriteLine("Would you like to play again?");
				Console.WriteLine("Press any key to continue. Type 'exit' to quit.");
				var playAgain = Console.ReadLine().ToLower();
				if (playAgain == "exit")
				{
					continuePlaying = false;
				}
			}

			Console.WriteLine("Good-bye!");
			Console.ReadKey();
		}

		static bool IsValidPermutation(string permutation, List<string> wordList)
		{
			return wordList.Any(w => w.StartsWith(permutation));
		}
		
		static List<string> GetPossibleWords(string currentWord, string availableLetters)
		{
			var rootPermutationList = new List<string>();
			var possiblePlayList = new List<string>();

			foreach (var letter in currentWord)
			{
				if (availableLetters.Contains('?'))
				{
					foreach (var character in _alphabetPointValues.Keys)
					{
						var wildCardAvailableLetters = availableLetters.Replace('?', character);
						var letterSet = (availableLetters + letter).ToCharArray();
						GetPermutationsAndWords(letterSet, "", _baseWordList, ref rootPermutationList, ref possiblePlayList);
					}
				}
				else
				{
					var letterSet = (availableLetters + letter).ToCharArray();
					GetPermutationsAndWords(letterSet, "", _baseWordList, ref rootPermutationList, ref possiblePlayList);
				}
			}

			return possiblePlayList;
		}

		static void GetPermutationsAndWords(char[] remainingLetters, string seedPermutation, List<string> wordList, ref List<string> rootPermutationList, ref List<string> possiblePlayList)
		{
			foreach (var letter in remainingLetters)
			{
				// TODO: Pass along the permutation match list so that you're only working on the permutations, not the entire word list

				var newPermutation = seedPermutation + letter;
				var permutationMatches = wordList.Where(a => a.StartsWith(newPermutation)).ToList();
				if (permutationMatches.Count() > 0 && !rootPermutationList.Contains(newPermutation))
				{
					Console.WriteLine($"New permutation found: {newPermutation}");
					rootPermutationList.Add(newPermutation);

					// Add the new permutation to the possiblePlayList if it's a word.
					if (permutationMatches.Contains(newPermutation) && !possiblePlayList.Contains(newPermutation))
					{
						Console.WriteLine($"New play found: {newPermutation}");
						possiblePlayList.Add(newPermutation);
					}

					var newRemainingLettersList = remainingLetters.ToList();
					newRemainingLettersList.Remove(letter);

					var newRemainingLetters = newRemainingLettersList.ToArray();
					if (newRemainingLetters.Length > 0)
					{
						GetPermutationsAndWords(newRemainingLetters, newPermutation, permutationMatches, ref rootPermutationList, ref possiblePlayList);
					}
				}
			}
		}

		static int CalculateScore(string word)
		{
			var totalScore = 0;

			char[] characters = word.ToCharArray();
			foreach (var character in characters)
			{
				totalScore += _alphabetPointValues[character];
			}
			totalScore = totalScore * characters.Length;

			return totalScore;
		}
	}
}

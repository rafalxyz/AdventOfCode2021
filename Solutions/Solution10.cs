using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution10 : ISolution
    {
        private static readonly Dictionary<char, char> ChunkPairs = new Dictionary<char, char>
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' },
        };

        private static readonly Dictionary<char, int> ScoreTable = new Dictionary<char, int>
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 },
        };

        private static readonly Dictionary<char, int> AutoCompletionScoreTable = new Dictionary<char, int>
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 },
        };

        private static bool IsOpeningChunk(char chunk) => ChunkPairs.Keys.Contains(chunk);
        private static bool IsClosingChunk(char chunk) => ChunkPairs.Values.Contains(chunk);

        private static char GetMatchingOpeningChunk(char closingChunk) => ChunkPairs.Single(kvp => kvp.Value == closingChunk).Key;
        private static char GetMatchingClosingChunk(char openingChunk) => ChunkPairs.Single(kvp => kvp.Key == openingChunk).Value;

        private static int GetScoreForIllegalChunk(char chunk) => ScoreTable[chunk];

        private static char? GetFirstInvalidChunk(string line)
        {
            var openingChunks = new List<char>();

            for (var i = 0; i < line.Length; i++)
            {
                if (IsClosingChunk(line[i]))
                {
                    var matchingOpeningChunk = GetMatchingOpeningChunk(line[i]);

                    var lastOpeningChunk = openingChunks.LastOrDefault();

                    if (lastOpeningChunk == default || lastOpeningChunk != matchingOpeningChunk)
                    {
                        return line[i];
                    }

                    openingChunks.RemoveAt(openingChunks.Count - 1);
                }
                else
                {
                    openingChunks.Add(line[i]);
                }
            }

            return null;
        }

        private static List<char> AutoComplete(string line)
        {
            var openingChunks = new List<char>();

            for (var i = 0; i < line.Length; i++)
            {
                if (IsClosingChunk(line[i]))
                {
                    openingChunks.RemoveAt(openingChunks.Count - 1);
                }
                else
                {
                    openingChunks.Add(line[i]);
                }
            }

            openingChunks.Reverse();

            return openingChunks.Select(GetMatchingClosingChunk).ToList();
        }

        private static long CalculateScoreForAutocomplete(List<char> completion)
        {
            return completion.Aggregate(0l, (acc, current) => acc * 5 + AutoCompletionScoreTable[current]);
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var totalScore = data
                .Select(GetFirstInvalidChunk)
                .Where(x => x != null)
                .Sum(x => GetScoreForIllegalChunk(x!.Value));
            
            return totalScore;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var scores = data
                .Where(x => GetFirstInvalidChunk(x) == default)
                .Select(AutoComplete)
                .Select(CalculateScoreForAutocomplete)
                .OrderBy(x => x)
                .ToList();

            return scores[scores.Count / 2];
        }
    }
}

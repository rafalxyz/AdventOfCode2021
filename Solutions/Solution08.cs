using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode21.Extensions;

namespace AdventOfCode21.Solutions
{
    internal class Solution08 : ISolution
    {
        private static Dictionary<int, string> Codes = new Dictionary<int, string>()
        {
            { 0, "abcefg" },
            { 1, "cf" },
            { 2, "acdeg" },
            { 3, "acdfg" },
            { 4, "bcdf" },
            { 5, "abdfg" },
            { 6, "abdefg" },
            { 7, "acf" },
            { 8, "abcdefg" },
            { 9, "abcdfg" },
        };

        class DataEntry
        {
            public List<Signal> Patterns { get; }

            public List<Signal> Outputs { get; }

            public Dictionary<char, char> Mapping { get; }

            public DataEntry(List<Signal> patterns, List<Signal> outputs)
            {
                Patterns = patterns;
                Outputs = outputs;

                var possibleMappings = new Dictionary<char, char[]>();

                var allSignals = patterns.Concat(outputs);

                foreach (var signal in allSignals)
                {
                    var possibleSegments = Codes.Where(x => x.Value.Count() == signal.Segments.Count()).SelectMany(x => x.Value.ToArray()).Distinct().ToArray();
                        
                    foreach (var segment in signal.Segments)
                    {
                        if (!possibleMappings.ContainsKey(segment))
                        {
                            possibleMappings[segment] = possibleSegments;
                        }
                        else
                        {
                            possibleMappings[segment] = possibleMappings[segment].Intersect(possibleSegments).ToArray();
                        }
                    }
                }

                var temp = possibleMappings.Select(x => Enumerable.Range(0, x.Value.Count()).ToList()).ToList();

                var cartesianProduct = temp.CartesianProduct().ToList();

                foreach (var product in cartesianProduct)
                {
                    var testMapping = possibleMappings
                        .Zip(product, (kvp, index) => KeyValuePair.Create(kvp.Key, kvp.Value[index]))
                        .ToDictionary(x => x.Key, x => x.Value);

                    if (allSignals.All(x => x.IsMappingCorrect(testMapping)))
                    {
                        Mapping = testMapping;
                        return;
                    }
                }

                throw new InvalidOperationException();
            }

            public int OutputValue => Outputs.Select(x => x.Decode(Mapping)).Aggregate(0, (acc, value) => acc * 10 + value);
        }

        record Signal(string Segments)
        {
            private static readonly int[] UniqueSegmentCounts = new int[] { 7, 4, 3, 2 };

            public bool HasUniqueSegmentCount => UniqueSegmentCounts.Contains(Segments.Count());

            public bool IsMappingCorrect(Dictionary<char, char> mapping)
            {
                return Codes.Select(x => x.Value).Contains(new string(Segments.Select(x => mapping[x]).OrderBy(x => x).ToArray()));
            }

            public int Decode(Dictionary<char, char> mapping)
            {
                return Codes.Single(x => x.Value == new string(Segments.Select(x => mapping[x]).OrderBy(x => x).ToArray())).Key;
            }
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var entries = ParseData(data);

            var result = entries
                .SelectMany(x => x.Outputs)
                .Where(output => output.HasUniqueSegmentCount)
                .Count();

            return result;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var entries = ParseData(data);

            var result = entries.Sum(x => x.OutputValue);

            return result;
        }

        private List<DataEntry> ParseData(IEnumerable<string> data)
        {
            return data.Select(line =>
            {
                var parts = line.Split(" | ");

                var patterns = parts[0].Trim(' ', '|').Split(' ').Select(x => new Signal(x)).ToList();
                var outputs = parts[1].Trim(' ', '|').Split(' ').Select(x => new Signal(x)).ToList();

                return new DataEntry(patterns, outputs);
            }).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution07 : ISolution
    {
        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var positions = ParseData(data);

            var minPosition = positions.Min();
            var maxPosition = positions.Max();

            var kvp = Enumerable.Range(minPosition, maxPosition + 1)
                .ToDictionary(x => x, x => positions.Sum(y => Math.Abs(y - x)))
                .OrderBy(x => x.Value)
                .First();

            return kvp.Value;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var positions = ParseData(data);

            var minPosition = positions.Min();
            var maxPosition = positions.Max();

            var kvp = Enumerable.Range(minPosition, maxPosition + 1)
                .ToDictionary(x => x, x => positions.Sum(y => Math.Abs(y - x) * (Math.Abs(y - x) + 1) / 2))
                .OrderBy(x => x.Value)
                .First();

            return kvp.Value;
        }

        private List<int> ParseData(IEnumerable<string> data)
        {
            return data.First().Split(',').Select(int.Parse).ToList();
        }
    }
}

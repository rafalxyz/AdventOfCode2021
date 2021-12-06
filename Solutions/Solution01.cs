using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution01 : ISolution
    {
        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var numbers = ParseData(data);

            return GetIncreaseCount(numbers, 1);
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var numbers = ParseData(data);

            return GetIncreaseCount(numbers, 3);
        }

        private List<int> ParseData(IEnumerable<string> data)
        {
            return data.Select(int.Parse).ToList();
        }

        private int GetIncreaseCount(List<int> numbers, int step = 1)
        {
            var result = 0;

            for (var i = step; i < numbers.Count; i++)
            {
                if (numbers[i] > numbers[i - step])
                {
                    result++;
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution03 : ISolution
    {
        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var numbers = data.ToArray();

            var mostCommonBits = GetMostCommonBits(numbers);

            var gammaBinary = string.Join("", mostCommonBits.Select(x => x.ToString()));

            var gamma = Convert.ToInt32(gammaBinary, 2);

            var epsilonBinary = string.Join("", gammaBinary.Select(x => x == '1' ? '0' : '1'));

            var epsilon = Convert.ToInt32(epsilonBinary, 2);

            return gamma * epsilon;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var numbers = data.ToList();

            var oxygenBinary = Filter(numbers, (@char, bit) => @char.ToString() == bit.ToString());

            var oxygen = Convert.ToInt32(oxygenBinary, 2);

            var co2Binary = Filter(numbers, (@char, bit) => @char.ToString() != bit.ToString());

            var co2 = Convert.ToInt32(co2Binary, 2);

            return oxygen * co2;
        }

        private string Filter(List<string> numbers, Func<char, int, bool> predicate)
        {
            var filteredNumbers = new List<string>(numbers);

            var i = 0;

            while (filteredNumbers.Count > 1)
            {
                var currentMostCommonBit = GetMostCommonBits(filteredNumbers.ToArray())[i];

                filteredNumbers = filteredNumbers.Where(x => predicate(x[i], currentMostCommonBit)).ToList();

                i++;
            }

            return filteredNumbers.Single();
        }

        private int[] GetMostCommonBits(string[] numbers)
        {
            var positiveBitCounts = new int[numbers[0].Length];

            for (int i = 0; i < positiveBitCounts.Length; i++)
            {
                for (int j = 0; j < numbers.Length; j++)
                {
                    if (numbers[j][i] == '1')
                    {
                        positiveBitCounts[i]++;
                    }
                }
            }

            return positiveBitCounts.Select(x => x >= numbers.Length / 2m ? 1 : 0).ToArray();
        }
    }
}

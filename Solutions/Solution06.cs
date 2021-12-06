using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution06 : ISolution
    {
        private class Fish
        {
            public int Counter { get; set; }
        }

        private class Simulation
        {
            /// <summary>
            /// Key - interval
            /// Value - count
            /// </summary>
            private Dictionary<int, long> _fishes;

            public Simulation(Dictionary<int, long> fishes)
            {
                _fishes = fishes;
            }

            public void MoveNext(int dayCount)
            {
                for (int i = 0; i < dayCount; i++)
                {
                    MoveNext();
                }
            }

            public void MoveNext()
            {
                var newFishes = new Dictionary<int, long>(_fishes);

                for (int i = 8; i >= 0; i--)
                {
                    newFishes[i] = newFishes[i] - _fishes[i];

                    if (i > 0)
                    {
                        newFishes[i - 1] = newFishes[i - 1] + _fishes[i];
                    }
                    else
                    {
                        newFishes[8] = newFishes[8] + _fishes[0];
                        newFishes[6] = newFishes[6] + _fishes[0];
                    }

                }

                _fishes = newFishes;
            }

            public void Write()
            {
                for (int i = 0; i <= 8; i++)
                {
                    if (_fishes[i] > 0)
                    {
                        Console.Write(new string(i.ToString()[0], (int)_fishes[i]));
                    }
                }
                Console.Write('\n');
            }

            public long FishCount => _fishes.Select(x => (long)x.Value).Sum();
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var fishes = ParseData(data);

            var simulation = new Simulation(fishes);

            simulation.Write();

            simulation.MoveNext(80);

            return simulation.FishCount;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var fishes = ParseData(data);

            var simulation = new Simulation(fishes);

            simulation.MoveNext(256);

            return simulation.FishCount;
        }

        private Dictionary<int, long> ParseData(IEnumerable<string> data)
        {
            var result = data.First().Split(',').Select(int.Parse).GroupBy(x => x).ToDictionary(x => x.Key, x => x.LongCount());

            for (int i = 0; i <= 8; i++)
            {
                if (!result.ContainsKey(i))
                {
                    result[i] = 0;
                }
            }

            return result;
        }
    }
}

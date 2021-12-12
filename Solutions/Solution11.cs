using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution11 : ISolution
    {
        class Simulation
        {
            public List<List<int>> Points { get; }

            public Simulation(List<List<int>> points)
            {
                Points = points;
            }

            public int Next()
            {
                var toFlash = new List<(int i, int j)>();

                var alreadyFLashed = new List<(int i, int j)>();

                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < Points[i].Count; j++)
                    {
                        Points[i][j]++;

                        if (Points[i][j] > 9)
                        {
                            toFlash.Add((i, j));
                        }
                    }
                }

                while (toFlash.Any())
                {
                    var point = toFlash.First();

                    Points[point.i][point.j] = 0;

                    toFlash.RemoveAt(0);

                    alreadyFLashed.Add(point);

                    var adjactedPoints = GetAdjacentPoints(point.i, point.j);

                    foreach (var adjacentPoint in adjactedPoints)
                    {
                        if (!alreadyFLashed.Contains(adjacentPoint))
                        {
                            Points[adjacentPoint.i][adjacentPoint.j]++;
                        }

                        if (Points[adjacentPoint.i][adjacentPoint.j] > 9 && !toFlash.Contains(adjacentPoint) && !alreadyFLashed.Contains(adjacentPoint))
                        {
                            toFlash.Add(adjacentPoint);
                        }
                    }
                }

                return alreadyFLashed.Count;
            }

            private bool IsValidPoint(int i, int j)
            {
                return i >= 0 && i <= Points.Count - 1 && j >= 0 && j <= Points[i].Count - 1;
            }

            private List<(int i, int j)> GetAdjacentPoints(int i, int j)
            {
                var potentialPoints = new List<(int i, int j)>
                {
                    (i, j - 1),
                    (i, j + 1),
                    (i - 1, j),
                    (i + 1, j),
                    (i + 1, j + 1),
                    (i + 1, j - 1),
                    (i - 1, j + 1),
                    (i - 1, j - 1),
                };

                return potentialPoints.Where(point => IsValidPoint(point.i, point.j)).ToList();
            }
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var simulation = ParseData(data);

            var result = 0;

            for (int i = 0; i < 100; i++)
            {
                result += simulation.Next();
            }

            return result;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var simulation = ParseData(data);

            var totalCount = simulation.Points.SelectMany(x => x).Count();

            int i = 0;

            while (true)
            {
                i++;

                var flashCount = simulation.Next();

                if (flashCount == totalCount)
                {
                    return i;
                }
            }
        }


        private Simulation ParseData(IEnumerable<string> data)
        {
            var points = data
                .Select(x => x.ToArray().Select(x => int.Parse(x.ToString())).ToList())
                .ToList();

            return new Simulation(points);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualBasic;

namespace AdventOfCode21.Solutions
{
    internal class Solution09 : ISolution
    {
        record HeightMap(List<List<int>> Points)
        {
            public List<(int i, int j)> GetLowPoints()
            {
                var lowPoints = new List<(int i, int j)>();

                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < Points[i].Count; j++)
                    {
                        var adjactentPoints = GetAdjacentPoints(i, j);

                        if (adjactentPoints.All(point => Points[point.i][point.j] > Points[i][j]))
                        {
                            lowPoints.Add((i, j));
                        }
                    }
                }

                return lowPoints;
            }

            public int GetLowPointsTotalRiskLevel(List<(int i, int j)> points)
            {
                return points.Select(point => Points[point.i][point.j]).Sum(x => x + 1);
            }

            public HashSet<(int i, int j)> GetBasinForLowPoint((int i, int j) point)
            {
                var basin = new HashSet<(int i, int j)>();

                var toCheck = new HashSet<(int i, int j)> { (point.i, point.j) };

                while (toCheck.Any())
                {
                    var item = toCheck.Last();
                    toCheck.Remove(item);

                    basin.Add(item);

                    var newPointsToCheck = GetAdjacentPoints(item.i, item.j)
                        .Where(x => !basin.Contains(x))
                        .Where(x => Points[x.i][x.j] != 9)
                        .ToList();

                    foreach (var pointToAdd in newPointsToCheck)
                    {
                        toCheck.Add(pointToAdd);
                    }
                }

                return basin;
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
                    (i + 1, j)
                };

                return potentialPoints.Where(point => IsValidPoint(point.i, point.j)).ToList();
            }
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var heightMap = ParseData(data);

            var lowPoints = heightMap.GetLowPoints();

            return heightMap.GetLowPointsTotalRiskLevel(lowPoints);
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var heightMap = ParseData(data);

            var lowPoints = heightMap.GetLowPoints();

            var result = lowPoints
                .Select(x => heightMap.GetBasinForLowPoint(x))
                .OrderByDescending(x => x.Count)
                .Take(3)
                .Aggregate(1, (acc, basin) => acc * basin.Count);

            return result;
        }

        private HeightMap ParseData(IEnumerable<string> data)
        {
            var points = data
                .Select(x => x.ToArray().Select(x => int.Parse(x.ToString())).ToList())
                .ToList();

            return new HeightMap(points);
        }
    }
}

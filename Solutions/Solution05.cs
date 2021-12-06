using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution05 : ISolution
    {
        private class Area
        {
            private List<PointWithCounter> _points = new List<PointWithCounter>();

            private int _minX;
            private int _maxX;
            private int _minY;
            private int _maxY;

            public Area(int minX, int maxX, int minY, int maxY)
            {
                _minX = minX;
                _maxX = maxX;
                _minY = minY;
                _maxY = maxY;

                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        _points.Add(new PointWithCounter(new Point(i, j)));
                    }
                }
            }

            public void CoverPoints(Line line)
            {
                var minX = Math.Min(line.Start.X, line.End.X);
                var maxX = Math.Max(line.Start.X, line.End.X);
                var minY = Math.Min(line.Start.Y, line.End.Y);
                var maxY = Math.Max(line.Start.Y, line.End.Y);

                foreach (var point in _points.Where(x => x.Point.X >= minX && x.Point.X <= maxX && x.Point.Y >= minY && x.Point.Y <= maxY))
                {
                    if (line.IsPointCovered(point.Point))
                    {
                        point.Count++;
                    }
                }
            }

            public int CountCoveredMoreThanTwice => _points.Count(x => x.Count >= 2);

            public void Print()
            {
                for (int i = _minY; i <= _maxY; i++)
                {
                    for (int j = _minX; j <= _maxX; j++)
                    {
                        var point = _points.Single(x => x.Point.X == j && x.Point.Y == i);
                        Console.Write(point.Count);
                    }

                    Console.Write("\n");
                }
            }
        }

        private class PointWithCounter
        {
            public Point Point { get; set; }

            public int Count { get; set; }

            public PointWithCounter(Point point)
            {
                Point = point;
            }
        }

        private record Point(int X, int Y);

        private record Line(Point Start, Point End)
        {
            public bool IsPointCovered(Point point)
            {
                if (Start.X == End.X)
                {
                    return point.X == Start.X && point.Y >= Math.Min(Start.Y, End.Y) && point.Y <= Math.Max(Start.Y, End.Y);
                }

                if (Start.Y == End.Y)
                {
                    return point.Y == Start.Y && point.X >= Math.Min(Start.X, End.X) && point.X <= Math.Max(Start.X, End.X);
                }

                int i = 0;
                int j = 0;
                var dimesion = Math.Abs(End.X - Start.X);

                while (Math.Abs(i) <= dimesion)
                {
                    if (point.X == Start.X + i && point.Y == Start.Y + j)
                    {
                        return true;
                    }

                    i = i + (End.X >= Start.X ? 1 : -1);
                    j = j + (End.Y >= Start.Y ? 1 : -1);
                }


                return false;
            }
                
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var lines = ParseData(data).Where(x => x.Start.X == x.End.X || x.Start.Y == x.End.Y).ToList();

            var minX = lines.SelectMany(x => new int[] { x.Start.X, x.End.X }).Min();
            var maxX = lines.SelectMany(x => new int[] { x.Start.X, x.End.X }).Max();
            var minY = lines.SelectMany(x => new int[] { x.Start.Y, x.End.Y }).Min();
            var maxY = lines.SelectMany(x => new int[] { x.Start.Y, x.End.Y }).Max();

            var area = new Area(minX, maxX, minY, maxY);

            foreach (var line in lines)
            {
                area.CoverPoints(line);
            }

            //area.Print();

            return area.CountCoveredMoreThanTwice;
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var lines = ParseData(data).ToList();

            var minX = lines.SelectMany(x => new int[] { x.Start.X, x.End.X }).Min();
            var maxX = lines.SelectMany(x => new int[] { x.Start.X, x.End.X }).Max();
            var minY = lines.SelectMany(x => new int[] { x.Start.Y, x.End.Y }).Min();
            var maxY = lines.SelectMany(x => new int[] { x.Start.Y, x.End.Y }).Max();

            var area = new Area(minX, maxX, minY, maxY);

            foreach (var line in lines)
            {
                area.CoverPoints(line);
            }

            return area.CountCoveredMoreThanTwice;
        }

        private List<Line> ParseData(IEnumerable<string> data)
        {
            return data.Select(x =>
            {
                var parts = x.Split(" -> ");

                var startCoords = parts[0].Split(',').Select(int.Parse).ToArray();
                var endCoords = parts[1].Split(',').Select(int.Parse).ToArray();

                var start = new Point(startCoords[0], startCoords[1]);
                var end = new Point(endCoords[0], endCoords[1]);

                return new Line(start, end);
            }).ToList();
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

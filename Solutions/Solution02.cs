using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution02 : ISolution
    {
        private enum OperationType
        {
            Forward = 0,
            Down = 1,
            Up = 2
        }

        private record Coords(int Horizontal, int Depth, int Aim = 0);



        private record Operation (OperationType Type, int Value)
        {
            public static Operation Parse(string data)
            {
                var parts = data.Split(' ');

                var type = ParseOperationType(parts[0]);
                var value = int.Parse(parts[1]);

                return new Operation(type, value);
            }

            private static OperationType ParseOperationType(string name)
            {
                return name switch
                {
                    "forward" => OperationType.Forward,
                    "down" => OperationType.Down,
                    "up" => OperationType.Up,
                    _ => throw new System.NotImplementedException()
                };
            }

            public Coords ApplyV1(Coords current)
            {
                var (horizontal, depth, _) = current;

                return Type switch
                {
                    OperationType.Forward => new Coords(horizontal + Value, depth),
                    OperationType.Up => new Coords(horizontal, depth - Value),
                    OperationType.Down => new Coords(horizontal, depth + Value),
                    _ => throw new System.NotImplementedException(),
                };
            }

            public Coords ApplyV2(Coords current)
            {
                var (horizontal, depth, aim) = current;

                return Type switch
                {
                    OperationType.Forward => new Coords(horizontal + Value, depth + aim * Value, aim),
                    OperationType.Up => new Coords(horizontal, depth, aim - Value),
                    OperationType.Down => new Coords(horizontal, depth, aim + Value),
                    _ => throw new System.NotImplementedException(),
                };
            }
        }
        

        public int GetFirstAnswer(IEnumerable<string> data)
        {
            var operations = ParseData(data);

            var coords = new Coords(0, 0);

            foreach (var operation in operations)
            {
                coords = operation.ApplyV1(coords);
            }

            return coords.Horizontal * coords.Depth;
        }

        public int GetSecondAnswer(IEnumerable<string> data)
        {
            var operations = ParseData(data);

            var coords = new Coords(0, 0);

            foreach (var operation in operations)
            {
                coords = operation.ApplyV2(coords);
            }

            return coords.Horizontal * coords.Depth;
        }

        private List<Operation> ParseData(IEnumerable<string> data)
        {
            return data.Select(Operation.Parse).ToList();
        }
    }
}

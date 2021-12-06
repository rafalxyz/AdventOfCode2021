using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode21.Solutions
{
    internal class Solution04 : ISolution
    {
        private class BoardSet
        {
            private IEnumerable<Board> _boards;

            public BoardSet(IEnumerable<Board> boards)
            {
                _boards = boards;
            }

            public void Mark(int number)
            {
                foreach (var board in _boards)
                {
                    board.Mark(number);
                }
            }

            public IEnumerable<Board> Winners
                => _boards.Where(x => x.IsWinner()).ToList();

            public bool HasAllBoardAlreadyWon => _boards.All(x => x.IsWinner());
        }

        private class Board
        {
            private const int Dimension = 5;

            public Guid Id { get; set; } = Guid.NewGuid();

            private List<List<BoardNumber>> _numbers;

            public Board(IEnumerable<int> numbers)
            {
                _numbers = numbers
                    .Select(x => new BoardNumber { Value = x, IsMarked = false })
                    .Chunk(Dimension)
                    .Select(x => new List<BoardNumber>(x))
                    .ToList();
            }

            public void Mark(int number)
            {
                foreach (var boardNumberRow in _numbers)
                {
                    foreach (var boardNumber in boardNumberRow)
                    {
                        if (boardNumber.Value == number)
                        {
                            boardNumber.IsMarked = true;
                        }
                    }
                }
            }

            public bool IsWinner()
            {
                for (int i = 0; i < Dimension; i++)
                {
                    var isRowMarked = true;
                    var isColumnMarked = true;

                    for (int j = 0; j < Dimension; j++)
                    {
                        if (!_numbers[i][j].IsMarked)
                        {
                            isRowMarked = false;
                        }

                        if (!_numbers[j][i].IsMarked)
                        {
                            isColumnMarked = false;
                        }
                    }

                    if (isRowMarked || isColumnMarked)
                    {
                        return true;
                    }
                }

                return false;
            }

            public int SumOfUmmarked => _numbers.SelectMany(x => x).Where(x => !x.IsMarked).Sum(x => x.Value);
        }

        private class BoardNumber
        {
            public int Value { get; set; }
            public bool IsMarked { get; set; }
        }

        public long GetFirstAnswer(IEnumerable<string> data)
        {
            var (inputs, boardSet) = ParseData(data);

            foreach (var input in inputs)
            {
                boardSet.Mark(input);

                var winner = boardSet.Winners.SingleOrDefault();

                if (winner != null)
                {
                    return winner.SumOfUmmarked * input;
                }
            }

            throw new InvalidOperationException();
        }

        public long GetSecondAnswer(IEnumerable<string> data)
        {
            var allWinners = new List<Board>();
            Board? latestWinner = null;
            int latestWinnerInput = 0;

            var (inputs, boardSet) = ParseData(data);

            foreach (var input in inputs)
            {
                if (boardSet.HasAllBoardAlreadyWon)
                {
                    break;
                }

                boardSet.Mark(input);

                foreach (var winner in boardSet.Winners)
                {
                    if (!allWinners.Any(x => x.Id == winner.Id))
                    {
                        allWinners.Add(winner);
                        latestWinner = winner;
                        latestWinnerInput = input;
                    }
                }
            }

            return latestWinner!.SumOfUmmarked * latestWinnerInput;
        }

        private (List<int> inputs, BoardSet boardSet) ParseData(IEnumerable<string> data)
        {
            var inputs = data.First().Split(',').Select(int.Parse).ToList();

            var boards = data
                .Skip(1)
                .Chunk(6)
                .Select(group => group
                    .Skip(1)
                    .Select(row => row.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
                    .SelectMany(x => x))
                .Select(numbers => new Board(numbers))
                .ToList();

            var boardSet = new BoardSet(boards);

            return (inputs, boardSet);
        }
    }
}

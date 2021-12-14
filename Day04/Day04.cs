using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day04 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var stringList = StreamParsers.GetStreamAsStringList(reader);
            var numbersDrawn = StringParsers.SplitDelimitedStringIntoIntList(stringList[0], ",");

            var boards = new List<BingoBoard>();
            do
            {
                var bingoBoard = new BingoBoard(StreamParsers.GetStreamAs2DIntArray(reader));
                boards.Add(bingoBoard);
            }
            while (!reader.EndOfStream);

            foreach (var n in numbersDrawn)
            {
                foreach (var board in boards)
                {
                    if (board.MarkSquare(n))
                    {
                        return (n * board.RemainingSum).ToString();
                    }
                }
            }

            throw new Exception("Unable to find solution");
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var stringList = StreamParsers.GetStreamAsStringList(reader);
            var numbersDrawn = StringParsers.SplitDelimitedStringIntoIntList(stringList[0], ",");

            var boards = new List<BingoBoard>();
            do
            {
                var bingoBoard = new BingoBoard(StreamParsers.GetStreamAs2DIntArray(reader));
                boards.Add(bingoBoard);
            }
            while (!reader.EndOfStream);

            var wonIndices = new HashSet<int>();
            foreach (var n in numbersDrawn)
            {
                for (int i = 0; i < boards.Count; ++i)
                {
                    if (wonIndices.Contains(i))
                    {
                        continue;
                    }

                    var board = boards[i];
                    if (board.MarkSquare(n))
                    {
                        wonIndices.Add(i);
                    }

                    if (wonIndices.Count == boards.Count)
                    {
                        return (n * board.RemainingSum).ToString();
                    }
                }
            }

            throw new Exception("Unable to find solution");
        }

        private class BingoBoard
        {
            private int dim;
            private int[][] board;
            private int[] rowTallies;
            private int[] colTallies;
            private IDictionary<int, int> rowLookup;
            private IDictionary<int, int> colLookup;

            public int RemainingSum { get; private set; }

            public BingoBoard(int[][] board)
            {
                this.board = board;
                dim = board.Length;
                rowTallies = new int[dim];
                colTallies = new int[dim];
                rowLookup = new Dictionary<int, int>();
                colLookup = new Dictionary<int, int>();

                for (int i = 0; i < dim; ++i)
                {
                    for (int j = 0; j < dim; ++j)
                    {
                        var square = board[i][j];
                        RemainingSum += square;
                        rowLookup.Add(square, i);
                        colLookup.Add(square, j);
                    }
                }
            }

            public bool MarkSquare(int n)
            {
                if (!rowLookup.ContainsKey(n))
                {
                    return false;
                }

                RemainingSum -= n;
                var row = rowLookup[n];
                var col = colLookup[n];
                rowTallies[row]++;
                colTallies[col]++;

                if (rowTallies[row] == dim || colTallies[col] == dim)
                {
                    return true;
                }

                return false;
            }
        }
    }
}

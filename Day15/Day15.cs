using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day15 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private Dictionary<(int, int), int> _minRiskDict;
        private int[][] _grid;

        private static readonly (int row, int col)[] NeighborDeltas = { (-1, 0), (0, 1), (1, 0), (0, -1) };

        public string SolveFirstStar(StreamReader reader)
        {
            _grid = StreamParsers.GetStreamAs2DIntArray(reader);
            _minRiskDict = new Dictionary<(int, int), int>();
            var min = FindMinRiskPath((0, 0), (_grid.Length - 1, _grid[0].Length - 1));
            return min.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            _grid = StreamParsers.GetStreamAs2DIntArray(reader);
            _minRiskDict = new Dictionary<(int, int), int>();
            var min = FindMinRiskPath((0, 0), (_grid.Length - 1, _grid[0].Length - 1), 5);
            return min.ToString();
        }

        private int FindMinRiskPath((int, int) start, (int, int) end, int n = 1)
        {
            var fakeHeap = new LinkedList<(int row, int col)>();

            fakeHeap.AddLast((start.Item1, start.Item2));
            _minRiskDict.Add(start, 0);
            var trueEnd = ((end.Item1 + 1) * n - 1, (end.Item2 + 1) * n - 1);

            while (fakeHeap.Count() > 0)
            {
                var minRiskCell = fakeHeap.RemoveAndReturnMin(x => _minRiskDict[x]);
                var cellRisk = _minRiskDict[minRiskCell];

                if (minRiskCell == trueEnd)
                {
                    return _minRiskDict[minRiskCell];
                }

                for (int i = 0; i < 4; ++i)
                {
                    var neighborDelta = NeighborDeltas[i];
                    var (row, col) = (minRiskCell.row + neighborDelta.row, minRiskCell.col + neighborDelta.col);

                    if (!IsValid(row, col, n))
                        continue;

                    var minRiskToCell = cellRisk + GridAt(row, col);
                    if (_minRiskDict.ContainsKey((row, col)))
                    {
                        if (minRiskToCell < _minRiskDict[(row, col)])
                        {
                            _minRiskDict[(row, col)] = minRiskToCell;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        _minRiskDict.Add((row, col), minRiskToCell);
                    }

                    fakeHeap.AddLast((row, col));
                }
            }

            return -1;
        }

        private int GridAt(int row, int col)
        {
            int vRow = row % _grid.Length;
            int vCol = col % _grid[0].Length;
            int offset = (row / _grid.Length) + (col / _grid[0].Length);
            var val = _grid[vRow][vCol] + offset;
            return (val > 9) ? val - 9 : val;
        }

        private bool IsValid(int row, int col, int n)
        {
            return row >= 0 && col >= 0 && row < _grid.Length * n && col < _grid[0].Length * n;
        }
    }
}

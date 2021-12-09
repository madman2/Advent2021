using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day09 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            string line;
            var grid = new List<int[]>();
            while ((line = reader.ReadLine()) != null)
            {
                grid.Add(Array.ConvertAll(line.ToCharArray(), c => (int)char.GetNumericValue(c)));
            }

            int riskLevel = 0;
            for (int row = 0; row < grid.Count; ++row)
            {
                for (int col = 0; col < grid[0].Length; ++col)
                {
                    var height = grid[row][col];
                    if (IsValueLower(grid, height, row - 1, col) &&
                        IsValueLower(grid, height, row, col - 1) &&
                        IsValueLower(grid, height, row, col + 1) &&
                        IsValueLower(grid, height, row + 1, col))
                    {
                        riskLevel += (height + 1);
                    }
                }
            }

            return riskLevel.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            string line;
            var grid = new List<int[]>();
            while ((line = reader.ReadLine()) != null)
            {
                grid.Add(Array.ConvertAll(line.ToCharArray(), c => (int)char.GetNumericValue(c)));
            }

            var lowPoints = new List<(int, int)>();
            for (int row = 0; row < grid.Count; ++row)
            {
                for (int col = 0; col < grid[0].Length; ++col)
                {
                    var n = grid[row][col];

                    if (IsValueLower(grid, n, row - 1, col) &&
                        IsValueLower(grid, n, row, col - 1) &&
                        IsValueLower(grid, n, row, col + 1) &&
                        IsValueLower(grid, n, row + 1, col))
                    {
                        lowPoints.Add((row, col));
                    }
                }
            }

            var basinSizes = new List<int>();
            foreach (var point in lowPoints)
            {
                basinSizes.Add(GetBasinSize(grid, point.Item1, point.Item2));
            }
            basinSizes.Sort();

            return (basinSizes[basinSizes.Count - 1] * basinSizes[basinSizes.Count - 2] * basinSizes[basinSizes.Count - 3]).ToString();
        }

        private bool IsValueLower(List<int[]> grid, int val, int row, int col)
        {
            if (row >= 0 && row < grid.Count && col >= 0 && col < grid[0].Length)
            {
                return grid[row][col] > val;
            }
            return true;
        }

        private int GetBasinSize(List<int[]> grid, int row, int col)
        {
            if (row < 0 ||
                col < 0 ||
                row == grid.Count ||
                col == grid[0].Length)
            {
                return 0;
            }

            int n = grid[row][col];
            grid[row][col] = 9;

            if (n == 9)
            {
                return 0;
            }

            int basinSize = 1;
            if (row > 0 && grid[row - 1][col] > n)
            {
                basinSize += GetBasinSize(grid, row - 1, col);
            }
            if (col > 0 && grid[row][col - 1] > n)
            {
                basinSize += GetBasinSize(grid, row, col - 1);
            }
            if (col < grid[0].Length - 1 && grid[row][col + 1] > n)
            {
                basinSize += GetBasinSize(grid, row, col + 1);
            }
            if (row < grid.Count - 1 && grid[row + 1][col] > n)
            {
                basinSize += GetBasinSize(grid, row + 1, col);
            }

            return basinSize;
        }
    }
}

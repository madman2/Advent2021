using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.IO;

namespace AdventOfCode
{
    public class Day11 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var grid = StreamParsers.GetStreamAs2DIntArray(reader);

            int flashCount = 0;
            for (int i = 1; i <= 100; ++i)
            {
                for (int row = 0; row < grid.Length; ++row)
                {
                    for (int col = 0; col < grid[0].Length; ++col)
                    {
                        grid[row][col]++;
                        if (grid[row][col] == 10)
                        {
                            flashCount += Flash(grid, row, col);
                        }
                    }
                }

                for (int row = 0; row < grid.Length; ++row)
                {
                    for (int col = 0; col < grid[0].Length; ++col)
                    {
                        if (grid[row][col] > 9)
                        {
                            grid[row][col] = 0;
                        }
                    }
                }
            }

            return flashCount.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var grid = StreamParsers.GetStreamAs2DIntArray(reader);

            int flashesThisStep = 0;
            int totalOctopuses = grid.Length * grid[0].Length;
            int step = 0;
            while (flashesThisStep != totalOctopuses)
            {
                step++;
                flashesThisStep = 0;
                for (int row = 0; row < grid.Length; ++row)
                {
                    for (int col = 0; col < grid[0].Length; ++col)
                    {
                        grid[row][col]++;

                        if (grid[row][col] == 10)
                        {
                            flashesThisStep += Flash(grid, row, col);
                        }
                    }
                }

                for (int row = 0; row < grid.Length; ++row)
                {
                    for (int col = 0; col < grid[0].Length; ++col)
                    {
                        if (grid[row][col] > 9)
                        {
                            grid[row][col] = 0;
                        }
                    }
                }
            }

            return step.ToString();
        }

        private int Flash(int[][] grid, int row, int col)
        {
            var flashes = 1;
            for (int i = row - 1; i <= row + 1; ++i)
            {
                for (int j = col - 1; j <= col + 1; ++j)
                {
                    if (i >= 0 && i < grid.Length && j >= 0 && j < grid[0].Length)
                    {
                        grid[i][j]++;
                        if (grid[i][j] == 10)
                        {
                            flashes += Flash(grid, i, j);
                        }
                    }
                }
            }

            return flashes;
        }
    }
}
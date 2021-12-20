using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day20 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var algorithm = reader.ReadLine().Select(x => x == '.' ? 0 : 1).ToArray();
            reader.ReadLine();
            var pixels = StreamParsers.GetStreamAs2DBitArray(reader);

            return Solve(algorithm, pixels, 2).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var algorithm = reader.ReadLine().Select(x => x == '.' ? 0 : 1).ToArray();
            reader.ReadLine();
            var pixels = StreamParsers.GetStreamAs2DBitArray(reader);

            return Solve(algorithm, pixels, 50).ToString();
        }

        private int Solve(int[] algorithm, int[][] pixels, int n)
        {
            var image = new Image(pixels);
            for (int i = 0; i < n; ++i)
            {
                image = image.Process(algorithm);
            }

            return image.NumLightPixels;
        }

        record Image(int[][] Pixels, int EmptySpace = 0)
        {
            public int Width { get; } = Pixels[0].Length;
            public int Height { get; } = Pixels.Length;
            public int NumLightPixels { get; } = Pixels.SelectMany(x => x).Sum();

            public int GetPixelAt(int row, int col) => IsValid(row, col) ? Pixels[row][col] : EmptySpace;
            public Image Process(int[] algorithm)
            {
                var result = new int[Height + 2][];
                for (int row = -1; row < Height + 1; ++row)
                {
                    result[row + 1] = new int[Width + 2];
                    for (int col = -1; col < Width + 1; ++col)
                    {
                        var surroundingPixels = string.Concat(
                            from i in Enumerable.Range(row - 1, 3)
                            from j in Enumerable.Range(col - 1, 3)
                            select GetPixelAt(i, j));
                        result[row + 1][col + 1] = algorithm[Convert.ToInt32(surroundingPixels, 2)];
                    }
                }

                return new(result, algorithm[EmptySpace * 511]);
            }

            private bool IsValid(int row, int col) => (row >= 0 && col >= 0 && row < Height && col < Width);
        }
    }
}

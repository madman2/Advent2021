using AdventOfCode.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day17 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var line = reader.ReadLine();
            var bounds = line.Substring(13).Split(",", StringSplitOptions.TrimEntries);
            var yMin = bounds[1].Substring(2).Split("..").Select(s => Convert.ToInt32(s)).First();
            var v0_Y = Math.Abs(yMin) - 1;
            return SumOneToN(v0_Y).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var line = reader.ReadLine();
            var bounds = line.Substring(13).Split(",", StringSplitOptions.TrimEntries);
            var xRange = bounds[0].Substring(2).Split("..").Select(s => Convert.ToInt32(s)).ToArray();
            var yRange = bounds[1].Substring(2).Split("..").Select(s => Convert.ToInt32(s)).ToArray();

            int count = 0;
            for (int v0_X = xRange[1]; v0_X >= 0; --v0_X)
            {
                if (SumOneToN(v0_X) < xRange[0])
                    break;

                for (int v0_Y = Math.Abs(yRange[0]) - 1; v0_Y >= yRange[0]; --v0_Y)
                {
                    count += Simulate(v0_X, v0_Y, xRange[0], xRange[1], yRange[0], yRange[1]);
                }
            }

            return count.ToString();
        }

        private int Simulate(int vX, int vY, int xMin, int xMax, int yMin, int yMax)
        {
            (int x, int y) = (0, 0);
            while (x < xMax && y > yMin)
            {
                (x, y) = (x + vX, y + vY);
                (vX, vY) = (Math.Max(0, vX - 1), vY - 1);

                if (x >= xMin && x <= xMax && y >= yMin && y <= yMax)
                    return 1;
            }
            return 0;
        }

        private int SumOneToN(int n)
        {
            return n * (n + 1) / 2;
        }
    }
}

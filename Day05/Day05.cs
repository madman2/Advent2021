using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day05 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var linesPerPoint = new Dictionary<ValueTuple<int, int>, int>();
            int overlapCount = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var splitLine = line.Split("->", StringSplitOptions.TrimEntries);
                var start = StringParsers.SplitDelimitedStringIntoStringList(splitLine[0], ",");
                var end = StringParsers.SplitDelimitedStringIntoStringList(splitLine[1], ",");

                var startTuple = (int.Parse(start[0]), int.Parse(start[1]));
                var endTuple = (int.Parse(end[0]), int.Parse(end[1]));

                overlapCount += DrawLine(startTuple, endTuple, linesPerPoint, false);
            }

            return overlapCount.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var linesPerPoint = new Dictionary<ValueTuple<int, int>, int>();
            int overlapCount = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var splitLine = line.Split("->", StringSplitOptions.TrimEntries);
                var start = StringParsers.SplitDelimitedStringIntoStringList(splitLine[0], ",");
                var end = StringParsers.SplitDelimitedStringIntoStringList(splitLine[1], ",");

                var startTuple = (int.Parse(start[0]), int.Parse(start[1]));
                var endTuple = (int.Parse(end[0]), int.Parse(end[1]));

                overlapCount += DrawLine(startTuple, endTuple, linesPerPoint, true);
            }

            return overlapCount.ToString();
        }

        private int DrawLine((int, int) start, (int, int) end, IDictionary<(int, int), int> linesPerPoint, bool diagEnabled)
        {
            int newPointsCovered = 0;
            if (start.Item1 == end.Item1 || start.Item2 == end.Item2 || diagEnabled)
            {
                var deltaX = end.Item1 - start.Item1;
                var deltaY = end.Item2 - start.Item2;

                var stepX = (deltaX == 0) ? 0 : (deltaX < 0) ? -1 : 1;
                var stepY = (deltaY == 0) ? 0 : (deltaY < 0) ? -1 : 1;
                
                var dist = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

                int x = start.Item1;
                int y = start.Item2;
                for (int i = 0; i <= dist; ++i)
                {
                    if (!linesPerPoint.ContainsKey((x, y)))
                    {
                        linesPerPoint[(x, y)] = 1;
                    }
                    else
                    {
                        linesPerPoint[(x, y)]++;
                        if (linesPerPoint[(x, y)] == 2)
                        {
                            newPointsCovered++;
                        }
                    }

                    x += stepX;
                    y += stepY;
                }
            }

            return newPointsCovered;
        }
    }
}

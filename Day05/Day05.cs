using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                var splitLine = line.Split(" ->", StringSplitOptions.TrimEntries);
                var start = StringParsers.SplitDelimitedStringIntoStringList(splitLine[0], ',');
                var end = StringParsers.SplitDelimitedStringIntoStringList(splitLine[1], ',');

                var startTuple = (int.Parse(start[0]), int.Parse(start[1]));
                var endTuple = (int.Parse(end[0]), int.Parse(end[1]));

                if (startTuple.Item1 == endTuple.Item1)
                {
                    for (int y = Math.Min(startTuple.Item2, endTuple.Item2); y <= Math.Max(startTuple.Item2, endTuple.Item2); ++y)
                    {
                        var point = (startTuple.Item1, y);
                        if (!linesPerPoint.ContainsKey(point))
                        {
                            linesPerPoint.Add(point, 1);
                        }
                        else
                        {
                            linesPerPoint[point]++;
                            if (linesPerPoint[point] == 2)
                            {
                                overlapCount++;
                            }
                        }
                    }
                }

                if (startTuple.Item2 == endTuple.Item2)
                {
                    for (int x = Math.Min(startTuple.Item1, endTuple.Item1); x <= Math.Max(startTuple.Item1, endTuple.Item1); ++x)
                    {
                        var point = (x, startTuple.Item2);
                        if (!linesPerPoint.ContainsKey(point))
                        {
                            linesPerPoint.Add(point, 1);
                        }
                        else
                        {
                            linesPerPoint[point]++;
                            if (linesPerPoint[point] == 2)
                            {
                                overlapCount++;
                            }
                        }
                    }
                }
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
                var splitLine = line.Split(" ->", StringSplitOptions.TrimEntries);
                var start = StringParsers.SplitDelimitedStringIntoStringList(splitLine[0], ',');
                var end = StringParsers.SplitDelimitedStringIntoStringList(splitLine[1], ',');

                var startTuple = (int.Parse(start[0]), int.Parse(start[1]));
                var endTuple = (int.Parse(end[0]), int.Parse(end[1]));

                if (startTuple.Item1 == endTuple.Item1)
                {
                    for (int y = Math.Min(startTuple.Item2, endTuple.Item2); y <= Math.Max(startTuple.Item2, endTuple.Item2); ++y)
                    {
                        var point = (startTuple.Item1, y);
                        if (!linesPerPoint.ContainsKey(point))
                        {
                            linesPerPoint.Add(point, 1);
                        }
                        else
                        {
                            linesPerPoint[point]++;
                            if (linesPerPoint[point] == 2)
                            {
                                overlapCount++;
                            }
                        }
                    }
                }
                else if (startTuple.Item2 == endTuple.Item2)
                {
                    for (int x = Math.Min(startTuple.Item1, endTuple.Item1); x <= Math.Max(startTuple.Item1, endTuple.Item1); ++x)
                    {
                        var point = (x, startTuple.Item2);
                        if (!linesPerPoint.ContainsKey(point))
                        {
                            linesPerPoint.Add(point, 1);
                        }
                        else
                        {
                            linesPerPoint[point]++;
                            if (linesPerPoint[point] == 2)
                            {
                                overlapCount++;
                            }
                        }
                    }
                }
                else
                {
                    int slope = -1;
                    if ((startTuple.Item1 < endTuple.Item1 && startTuple.Item2 < endTuple.Item2) ||
                        startTuple.Item1 > endTuple.Item1 && startTuple.Item2 > endTuple.Item2)
                    {
                        slope = 1;
                    }

                    int y = (startTuple.Item1 < endTuple.Item1) ? startTuple.Item2 : endTuple.Item2;
                    for (int x = Math.Min(startTuple.Item1, endTuple.Item1); x <= Math.Max(startTuple.Item1, endTuple.Item1); ++x)
                    {
                        var point = (x, y);
                        if (!linesPerPoint.ContainsKey(point))
                        {
                            linesPerPoint.Add(point, 1);
                        }
                        else
                        {
                            linesPerPoint[point]++;
                            if (linesPerPoint[point] == 2)
                            {
                                overlapCount++;
                            }
                        }
                        y += slope;
                    }
                }
            }

            return overlapCount.ToString();
        }
    }
}

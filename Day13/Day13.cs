using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day13 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var points = StreamParsers.GetStreamAsListOfDelimitedInts(reader, ',').Select(x => (x[0], x[1])).ToList();
            var (dir, val) = ParseFold(reader.ReadLine());
            var foldResult = Fold(dir, val, points);
            return foldResult.Count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var points = StreamParsers.GetStreamAsListOfDelimitedInts(reader, ',').Select(x => (x[0], x[1])).ToList();

            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var (dir, val) = ParseFold(line);
                points = Fold(dir, val, points);
            }

            return PrintPointsToString(points);
        }

        private List<(int, int)> Fold(char dir, int val, List<(int, int)> inputPoints)
        {
            var foldResult = new HashSet<(int, int)>();
            foreach (var (x, y) in inputPoints)
            {
                foldResult.Add(((dir == 'x' && x > val) ? (2 * val - x) : x, (dir == 'y' && y > val) ? (2 * val - y) : y));
            }
            return foldResult.ToList();
        }

        private string PrintPointsToString(List<(int, int)> points)
        {
            var pointsSet = new HashSet<(int, int)>(points);
            var xMax = points.OrderByDescending(x => x.Item1).First().Item1;
            var yMax = points.OrderByDescending(x => x.Item2).First().Item2;
            var result = "\n";
            for (int y = 0; y <= yMax; ++y)
            {
                for (int x = 0; x <= xMax; ++x)
                {
                    result += pointsSet.Contains((x, y)) ? '#' : ' ';
                }
                result += '\n';
            }

            return result;
        }

        private (char, int) ParseFold(string line)
        {
            var words = StringParsers.SplitDelimitedStringIntoStringList(line);
            var fold = StringParsers.SplitDelimitedStringIntoStringList(words[2], '=');
            return (fold[0].First(), Convert.ToInt32(fold[1]));
        }
    }
}

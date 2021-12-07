using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day07 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var intList = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ',');
            intList.Sort();

            var median = intList[intList.Count / 2];
            int fuel = 0;
            foreach (var n in intList)
            {
                fuel += Math.Abs(n - median);
            }

            return fuel.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var intList = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ',');

            int minFuel = int.MaxValue;
            for (int pos = 0; pos <= intList.Max(); ++pos)
            {
                int fuel = 0;
                foreach (var n in intList)
                {
                    var dist = Math.Abs(n - pos);
                    var cost = (1 + dist) * dist / 2;
                    fuel += cost;
                }
                if (fuel < minFuel)
                {
                    minFuel = fuel;
                }
            }

            return minFuel.ToString();
        }
    }
}

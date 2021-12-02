using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.IO;

namespace AdventOfCode
{
    public class Day02 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var instructionList = StreamParsers.GetStreamAsListOfDelimitedStrings(reader, new char[] {' '});

            int horizontal = 0;
            int depth = 0;
            foreach (var instruction in instructionList)
            {
                var units = int.Parse(instruction[1]);
                if (instruction[0] == "forward")
                    horizontal += units;
                else if (instruction[0] == "down")
                    depth += units;
                else
                    depth -= units;
            }

            return (horizontal * depth).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var numberList = StreamParsers.GetStreamAsListOfDelimitedStrings(reader, new char[] {' '});

            int horizontal = 0;
            int depth = 0;
            int aim = 0;
            foreach (var instruction in numberList)
            {
                var units = int.Parse(instruction[1]);
                if (instruction[0] == "forward")
                {
                    horizontal += units;
                    depth += units * aim;
                }
                else if (instruction[0] == "down")
                    aim += units;
                else
                    aim -= units;
            }

            return (horizontal * depth).ToString();
        }
    }
}

using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.IO;

namespace AdventOfCode
{
    public class Day01 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var numberList = StreamParsers.GetStreamAsIntList(reader);

            int count = 0;
            for (int i = 1; i < numberList.Count; ++i)
            {
                if (numberList[i] > numberList[i - 1])
                    count++;
            }

            return count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var numberList = StreamParsers.GetStreamAsIntList(reader);

            int count = 0;
            for (int i = 3; i < numberList.Count; i++)
            {
                if (numberList[i] > numberList[i - 3])
                    count++;
            }

            return count.ToString();
        }
    }
}

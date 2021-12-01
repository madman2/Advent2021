using System.IO;

namespace AdventOfCode.Interfaces
{
    interface ISolver
    {
        public string InputFileName { get; }

        public string SolveFirstStar(StreamReader reader);
        public string SolveSecondStar(StreamReader reader);
    }
}

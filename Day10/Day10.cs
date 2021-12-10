using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day10 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private static Dictionary<char, char> OpeningToClosingLookup = new Dictionary<char, char>
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' },
        };

        private static Dictionary<char, int>[] ClosingCharPoints = new Dictionary<char, int>[2]
        { 
            new Dictionary<char, int>
            {
                { ')', 3 },
                { ']', 57 },
                { '}', 1197 },
                { '>', 25137 }
            },
            new Dictionary<char, int>
            {
                { ')', 1 },
                { ']', 2 },
                { '}', 3 },
                { '>', 4 }
            }
        };

        public string SolveFirstStar(StreamReader reader)
        {
            var stringList = StreamParsers.GetStreamAsStringList(reader);

            var score = 0;
            foreach (var line in stringList)
            {
                var s = new Stack<char>();
                foreach (var c in line)
                {
                    if (OpeningToClosingLookup.ContainsKey(c))
                    {
                        s.Push(c);
                    }
                    else
                    {
                        if (s.Count == 0)
                        {
                            score += ClosingCharPoints[0][c];
                            break;
                        }

                        var top = s.Pop();

                        if (OpeningToClosingLookup[top] != c)
                        {
                            score += ClosingCharPoints[0][c];
                            break;
                        }
                    }
                }
            }

            return score.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var stringList = StreamParsers.GetStreamAsStringList(reader);

            var scores = new List<long>();
            foreach (var line in stringList)
            {
                var s = new Stack<char>();
                var invalidLine = false;
                foreach (var c in line)
                {
                    if (OpeningToClosingLookup.ContainsKey(c))
                    {
                        s.Push(c);
                    }
                    else
                    {
                        if (s.Count == 0)
                        {
                            invalidLine = true;
                            break;
                        }

                        var top = s.Pop();

                        if (OpeningToClosingLookup[top] != c)
                        {
                            invalidLine = true;
                            break;
                        }
                    }
                }

                if (invalidLine)
                {
                    continue;
                }

                if (s.Count > 0)
                {
                    var closingString = new string(s.Select(x => OpeningToClosingLookup[x]).ToArray());
                    scores.Add(ClosingStringScore(closingString));
                }
            }

            scores.Sort();
            return scores[scores.Count / 2].ToString();
        }

        private long ClosingStringScore(string str)
        {
            long sum = 0;
            foreach (var c in str)
            {
                sum *= 5;
                sum += ClosingCharPoints[1][c];
            }

            return sum;
        }
    }
}

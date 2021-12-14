using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day14 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private Dictionary<(char, char), char> Template;
        private Dictionary<char, long> ElementCounts;
        private List<Dictionary<(char, char), long>> PairCountByStep;

        public string SolveFirstStar(StreamReader reader)
        {
            return Solve(reader, 10);
        }

        public string SolveSecondStar(StreamReader reader)
        {
            return Solve(reader, 40);
        }

        private string Solve(StreamReader reader, int steps)
        {
            var polymer = reader.ReadLine();
            reader.ReadLine();

            var lines = StreamParsers.GetStreamAsStringList(reader);
            Template = lines.Select(x => StringParsers.SplitDelimitedStringIntoStringList(x, "->")).ToDictionary(s => (s[0][0], s[0][1]), s => s[1].First());

            PairCountByStep = new List<Dictionary<(char, char), long>>();
            ElementCounts = new Dictionary<char, long>();

            PairCountByStep.Add(new Dictionary<(char, char), long>());
            ElementCounts.Add(polymer.First(), 1);
            for (int i = 0; i < polymer.Length; ++i)
            {
                var e = polymer[i];
                if (!ElementCounts.ContainsKey(e))
                {
                    ElementCounts.Add(e, 0);
                }
                ElementCounts[e]++;

                if (i < polymer.Length - 1)
                {
                    var pair = (polymer[i], polymer[i + 1]);
                    if (!PairCountByStep.First().ContainsKey(pair))
                    {
                        PairCountByStep.First().Add(pair, 0);
                    }
                    PairCountByStep.First()[pair]++;
                }
            }

            for (int i = 1; i <= steps; ++i)
            {
                DoPairInsertion(i);
            }

            return (ElementCounts.Values.Max() - ElementCounts.Values.Min()).ToString();
        }

        private void DoPairInsertion(int step)
        {
            var lastStepPairCounts = PairCountByStep[step - 1];
            var newPairCounts = new Dictionary<(char, char), long>();
            foreach (var pair in lastStepPairCounts.Keys)
            {
                long n = lastStepPairCounts[pair];

                var newElement = Template[pair];
                var leftPair = (pair.Item1, newElement);
                var rightPair = (newElement, pair.Item2);

                if (!newPairCounts.ContainsKey(leftPair))
                {
                    newPairCounts.Add(leftPair, 0);
                }

                if (!newPairCounts.ContainsKey(rightPair))
                {
                    newPairCounts.Add(rightPair, 0);
                }

                newPairCounts[leftPair] += n;
                newPairCounts[rightPair] += n;

                if (!ElementCounts.ContainsKey(newElement))
                {
                    ElementCounts.Add(newElement, 0);
                }
                ElementCounts[newElement] += n;
            }
            PairCountByStep.Add(newPairCounts);
        }
    }
}

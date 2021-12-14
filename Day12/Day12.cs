using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day12 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var G = new GraphDay12();

            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var s = StringParsers.SplitDelimitedStringIntoStringList(line, "-");
                if (s[0] != "end" && s[1] != "start")
                {
                    G.AddEdge(s[0], s[1]);
                }
                if (s[1] != "end" && s[0] != "start")
                {
                    G.AddEdge(s[1], s[0]);
                }
            }

            var paths = G.CountPaths("start", "end", 1);
            return paths.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var G = new GraphDay12();

            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var s = StringParsers.SplitDelimitedStringIntoStringList(line, "-");
                if (s[0] != "end" && s[1] != "start")
                {
                    G.AddEdge(s[0], s[1]);
                }
                if (s[1] != "end" && s[0] != "start")
                {
                    G.AddEdge(s[1], s[0]);
                }
            }

            var paths = G.CountPaths("start", "end", 2);
            return paths.ToString();
        }

        private class GraphDay12 : GraphBase<string>
        {
            public GraphDay12() : base(true) { }

            public int CountPaths(string start, string end, int allowedVisitsSmallCave = 1)
            {
                return CountPaths(start, end, allowedVisitsSmallCave, new Dictionary<string, int>());
            }

            private int CountPaths(string start, string end, int allowedVisitsSmallCave, IDictionary<string, int> seenTimes)
            {
                if (char.IsLower(start.First()))
                {
                    if (!seenTimes.ContainsKey(start))
                    {
                        seenTimes[start] = 0;
                    }
                    seenTimes[start]++;
                }

                int paths = 0;
                if (start == end)
                {
                    paths = 1;
                }
                else
                {
                    int maxVisits = seenTimes.Values.Max();
                    foreach (var E in _adj[start])
                    {
                        var v = E.Vertex;

                        if (seenTimes.ContainsKey(v) && seenTimes[v] > 0)
                        {
                            if (seenTimes[v] == allowedVisitsSmallCave)
                                continue;
                            if (maxVisits == allowedVisitsSmallCave)
                                continue;
                        }

                        paths += CountPaths(v, end, allowedVisitsSmallCave, seenTimes);
                    }
                }

                if (seenTimes.ContainsKey(start))
                {
                    seenTimes[start]--;
                }

                return paths;
            }
        }
    }
}

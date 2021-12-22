using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day22 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);

            return Solve(lines, 50).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);

            return Solve(lines).ToString();
        }

        private long Solve(List<string> lines, int radius = int.MaxValue)
        {
            var prisms = new List<RectPrism>();
            foreach (var line in lines)
            {
                var isOn = line.StartsWith("on");
                var bounds = line.Substring(isOn ? 3 : 4).Split(',')
                                .Select(x => x.Substring(2).Split("..")
                                .Select(int.Parse).ToArray()).ToArray();
                
                var maxBoundary = bounds.SelectMany(x => x).Max(x => Math.Abs(x));
                if (maxBoundary > radius)
                    continue;

                var newPrism = new RectPrism((bounds[0][0], bounds[0][1]),
                                       (bounds[1][0], bounds[1][1]),
                                       (bounds[2][0], bounds[2][1]));

                var newPrisms = new List<RectPrism>();
                foreach (var c in prisms)
                {
                    newPrisms.AddRange(newPrism.Difference(c));
                }
                if (isOn)
                {
                    newPrisms.Add(newPrism);
                }
                prisms = newPrisms;
            }

            return prisms.Sum(p => p.Volume);
        }

        record RectPrism((int min, int max) X, (int min, int max) Y, (int min, int max) Z)
        {
            public long Volume { get; } = (long)(X.max - X.min + 1) * (Y.max - Y.min + 1) * (Z.max - Z.min + 1);
            public bool IsValid { get; } = X.max >= X.min && Y.max >= Y.min && Z.max >= Z.min;

            public RectPrism Intersection(RectPrism other)
            {
                return new((Math.Max(X.min, other.X.min), Math.Min(X.max, other.X.max)),
                           (Math.Max(Y.min, other.Y.min), Math.Min(Y.max, other.Y.max)),
                           (Math.Max(Z.min, other.Z.min), Math.Min(Z.max, other.Z.max)));
            }

            public IEnumerable<RectPrism> Difference(RectPrism other)
            {
                var intersection = Intersection(other);

                if (!intersection.IsValid)
                    return new List<RectPrism> { other };
                else if (intersection == other)
                    return new List<RectPrism>();

                var dPrisms = new List<RectPrism>();
                dPrisms.Add(new RectPrism((other.X.min, X.min - 1), other.Y, other.Z));
                dPrisms.Add(new RectPrism((X.max + 1, other.X.max), other.Y, other.Z));
                dPrisms.Add(new RectPrism(intersection.X, (Y.max + 1, other.Y.max), other.Z));
                dPrisms.Add(new RectPrism(intersection.X, (other.Y.min, Y.min - 1), other.Z));
                dPrisms.Add(new RectPrism(intersection.X, intersection.Y, (Z.max + 1, other.Z.max)));
                dPrisms.Add(new RectPrism(intersection.X, intersection.Y, (other.Z.min, Z.min - 1)));

                return dPrisms.Where(p => p.IsValid);
            }
        }
    }
}

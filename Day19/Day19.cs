using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day19 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private IDictionary<int, Scanner> FoundScanners;
        private Dictionary<int, List<Scanner>> UnknownScanners;

        public string SolveFirstStar(StreamReader reader)
        {
            FoundScanners = new Dictionary<int, Scanner>();
            UnknownScanners = new Dictionary<int, List<Scanner>>();

            ParseScanners(reader);

            FoundScanners.Add(0, UnknownScanners[0].First());
            UnknownScanners.Remove(0);

            var q = new Queue<int>();
            q.Enqueue(0);

            while (q.Count > 0)
            {
                var scanner = FoundScanners[q.Dequeue()];
                var overlappingScanners = FindOverlappingScanners(scanner);

                foreach (var overlappingScanner in overlappingScanners)
                {
                    FoundScanners.Add(overlappingScanner.Id, overlappingScanner);
                    UnknownScanners.Remove(overlappingScanner.Id);

                    q.Enqueue(overlappingScanner.Id);
                }
            }

            var uniqueBeacons = FoundScanners.Values.SelectMany(s => s.AbsoluteBeacons).ToHashSet();

            return uniqueBeacons.Count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            FoundScanners = new Dictionary<int, Scanner>();
            UnknownScanners = new Dictionary<int, List<Scanner>>();

            ParseScanners(reader);

            FoundScanners.Add(0, UnknownScanners[0].First());
            UnknownScanners.Remove(0);

            var q = new Queue<int>();
            q.Enqueue(0);

            while (q.Count > 0)
            {
                var scanner = FoundScanners[q.Dequeue()];
                var overlappingScanners = FindOverlappingScanners(scanner);

                foreach (var overlappingScanner in overlappingScanners)
                {
                    FoundScanners.Add(overlappingScanner.Id, overlappingScanner);
                    UnknownScanners.Remove(overlappingScanner.Id);

                    q.Enqueue(overlappingScanner.Id);
                }
            }

            var maxDistance = (from a in FoundScanners.Values
                               from b in FoundScanners.Values
                               select a.DistanceTo(b)).Max();

            return maxDistance.ToString();
        }

        private void ParseScanners(StreamReader reader)
        {
            int id = 0;
            do
            {
                var relativeScanners = new List<Scanner>();
                var relativeBeaconsLists = new List<Vector>[24];
                reader.ReadLine();
                var beaconsStringList = StreamParsers.GetStreamAsStringList(reader);
                foreach (var beaconString in beaconsStringList)
                {
                    var coordsArr = beaconString.Split(',');
                    var beacon = new Vector(int.Parse(coordsArr[0]), int.Parse(coordsArr[1]), int.Parse(coordsArr[2]));
                    var relativeBeacons = beacon.GenerateAllRotations().ToList();
                    for (int i = 0; i < relativeBeaconsLists.Length; ++i)
                    {
                        if (relativeBeaconsLists[i] == null)
                            relativeBeaconsLists[i] = new List<Vector>();
                        relativeBeaconsLists[i].Add(relativeBeacons[i]);
                    }
                }
                relativeScanners.AddRange(relativeBeaconsLists.Select(x => new Scanner(id, x)));
                UnknownScanners.Add(id, relativeScanners);
                id++;
            } while (!reader.EndOfStream);
        }

        private IEnumerable<Scanner> FindOverlappingScanners(Scanner targetScanner)
        {
            foreach (var unknownScanner in UnknownScanners.Values)
            {
                foreach (var possibleScanner in unknownScanner)
                {
                    var diffVectors = new Dictionary<Vector, int>();
                    foreach (var possibleBeacon in possibleScanner.RelativeBeacons)
                    {
                        foreach (var targetBeacon in targetScanner.AbsoluteBeacons)
                        {
                            var diff = targetBeacon.Subtract(possibleBeacon);
                            if (!diffVectors.ContainsKey(diff))
                                diffVectors.Add(diff, 0);
                            diffVectors[diff]++;
                        }
                    }
                    var scannerPosition = diffVectors.Keys.Where(k => diffVectors[k] >= 12);
                    if (scannerPosition.Count() > 0)
                    {
                        yield return possibleScanner with { Position = scannerPosition.First() };
                    }
                }
            }
        }

        record Scanner(int Id, List<Vector> RelativeBeacons, Vector Position = default)
        {
            public IEnumerable<Vector> AbsoluteBeacons => RelativeBeacons.Select(x => x.Add(Position));

            public int DistanceTo(Scanner other) => Position.ManhattanDistance(other.Position);
        }

        record struct Vector(int X, int Y, int Z)
        {
            public Vector Subtract(Vector other) => new(X - other.X, Y - other.Y, Z - other.Z);

            public Vector Add(Vector other) => new(X + other.X, Y + other.Y, Z + other.Z);

            public int ManhattanDistance(Vector other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);

            /*
             * Generates an IEnumerable of Vectors containing the Vectors that result when
             * this Vector is oriented in all 24 possible orientations that can result by
             * rotating it about the three axes.
             */
            public IEnumerable<Vector> GenerateAllRotations()
            {
                return GenerateAllUpOrientations(this).SelectMany(v => GenerateAllRotationsAboutVerticalAxis(v));
            }

            /* 
             * Generates an IEnumerable of Vectors containing the Vectors that result when
             * the input Vector is rotated such that "up" aligns with each of the following directions:
             * 
             * [ +Z, -Z, +Y, -Y, +X, -X ]
             * 
             * This is achieved by doing the following three times:
             * 
             * 1. Yield the current Vector, which is oriented such that some direction D faces up (X, Y, Z)
             * 2. Yield the current Vector rotated 180' about a horizontal axis such that direction -D faces up (X, -Y, -Z)
             * 3. Rotate the current Vector 90' clockwise about a positive horizontal axis (X, -Z, Y)
             * 4. Rotate the current Vector 90' clockwise about the positive vertical axis (Z, X, Y)
             * 
             * The resulting Vectors, in terms of X, Y, Z values of the input Vector:
             * 
             *  {
             *      (X, Y, Z),
             *      (X -Y, -Z),
             *      (Z, X, Y),
             *      (Z, -X, -Y),
             *      (Y, Z, X),
             *      (Y, -Z, -X)
             *  }
             */
            private static IEnumerable<Vector> GenerateAllUpOrientations(Vector v)
            {
                var current = v;
                for (int i = 0; i < 3; ++i)
                {
                    yield return current;
                    yield return new Vector(current.X, -current.Y, -current.Z);
                    current = new(current.Z, current.X, current.Y);
                }
            }

            /* 
             * Generates an IEnumerable of Vectors containing the Vectors that result when the
             * input Vector is rotated into all unique orientations about the vertical axis
             *              * 
             * The resulting Vectors, in terms of X, Y, Z values of the input Vector:
             * 
             *  {
             *      (X, Y, Z),
             *      (Y -X, Z),
             *      (-X, -Y, Z),
             *      (-Y, X, Z)
             *  }
            */
            private static IEnumerable<Vector> GenerateAllRotationsAboutVerticalAxis(Vector v)
            {
                var current = v;
                for (int j = 0; j < 4; ++j)
                {
                    yield return current;
                    current = new Vector(current.Y, -current.X, current.Z);
                }
            }
        }
    }
}

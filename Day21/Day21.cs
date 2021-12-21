using AdventOfCode.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day21 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private Dictionary<int, int> PossibleRollSums;
        private Dictionary<(int, int, int, int, int), (long p1, long p2)> WinLookup;

        public string SolveFirstStar(StreamReader reader)
        {
            var p1Start = int.Parse(reader.ReadLine().Split(' ').Last());
            var p2Start = int.Parse(reader.ReadLine().Split(' ').Last());

            var result = PlayDeterministic(p1Start, p2Start, 0);

            return (result.loserScore * result.diceRolls).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var p1Start = int.Parse(reader.ReadLine().Split(' ').Last());
            var p2Start = int.Parse(reader.ReadLine().Split(' ').Last());

            var result = PlayDirac(p1Start, p2Start, 0);

            return result.Max().ToString();
        }

        private (int loserScore, int diceRolls) PlayDeterministic(int p1Start, int p2Start, int firstTurn)
        {
            var pos = new int[] { p1Start, p2Start };
            var score = new int[] { 0, 0 };
            var turn = firstTurn;

            int diceNextValue = 1;
            int diceRolls = 0;
            while (score[0] < 1000 && score[1] < 1000)
            {
                pos[turn] = ((pos[turn] + 3 * diceNextValue + 2) % 10) + 1;
                score[turn] += pos[turn];

                diceNextValue += 3;
                diceRolls += 3;
                turn = (turn + 1) % 2;
            }

            return (score.Min(), diceRolls);
        }

        private long[] PlayDirac(int p1Start, int p2Start, int firstTurn)
        {
            var possibleRolls = from i in Enumerable.Range(1, 3)
                                from j in Enumerable.Range(1, 3)
                                from k in Enumerable.Range(1, 3)
                                select new int[3] { i, j, k };

            WinLookup = new Dictionary<(int, int, int, int, int), (long, long)>();
            PossibleRollSums = possibleRolls.GroupBy(i => i.Sum())
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            return PlayDirac(0, new int[] { p1Start, p2Start }, new int[2]);
        }

        private long[] PlayDirac(int turn, int[] pos, int[] score)
        {
            if (WinLookup.ContainsKey((turn, pos[0], pos[1], score[0], score[1])))
            {
                var playResult = WinLookup[(turn, pos[0], pos[1], score[0], score[1])];
                return new long[] { playResult.p1, playResult.p2 };
            }

            var wins = new long[2];
            foreach (var r in PossibleRollSums.Keys)
            {
                var rollSum = r;

                var newPos = new int[2] { pos[0], pos[1] };
                var newScore = new int[2] { score[0], score[1] };

                newPos[turn] = ((pos[turn] + rollSum - 1) % 10) + 1;
                newScore[turn] = score[turn] + newPos[turn];

                if (newScore[turn] >= 21)
                {
                    wins[turn] += PossibleRollSums[rollSum];
                    continue;
                }

                wins = wins.Zip(PlayDirac((turn + 1) % 2, newPos, newScore), (a, b) => a + PossibleRollSums[rollSum] * b).ToArray();
            }

            WinLookup.Add((turn, pos[0], pos[1], score[0], score[1]), (wins[0], wins[1]));
            return wins;
        }
    }
}

using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day06 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private const int SpawnCycle = 7;
        private const int FirstCycleDelay = 2;

        public string SolveFirstStar(StreamReader reader)
        {
            var initialFishList = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ',');

            return PredictLanternfish(initialFishList, 80).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var initialFishList = StringParsers.SplitDelimitedStringIntoIntList(reader.ReadLine(), ',');

            return PredictLanternfish(initialFishList, 256).ToString();
        }

        private long PredictLanternfish(List<int> initialFishList, int days)
        {
            var fishTimerCounts = new Dictionary<int, int>();
            foreach (var fishTimer in initialFishList)
            {
                if (!fishTimerCounts.ContainsKey(fishTimer))
                {
                    fishTimerCounts.Add(fishTimer, 1);
                }
                else
                {
                    fishTimerCounts[fishTimer]++;
                }
            }

            var spawnPerDay = new long[days + 1];
            foreach (var fishTimer in fishTimerCounts.Keys)
            {
                for (int i = fishTimer + 1; i <= days; i += SpawnCycle)
                {
                    spawnPerDay[i] += fishTimerCounts[fishTimer];
                }
            }

            for (var day = 1; day <= days; ++day)
            {
                for (int i = day + SpawnCycle + FirstCycleDelay; i <= days; i += SpawnCycle)
                {
                    spawnPerDay[i] += spawnPerDay[day];
                }
            }

            return initialFishList.Count + spawnPerDay.Sum();
        }
    }
}

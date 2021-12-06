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
            var spawnPerDay = new long[days + 1];
            foreach (var fishTimer in initialFishList)
            {
                for (int i = fishTimer + 1; i <= days; i += SpawnCycle)
                {
                    spawnPerDay[i]++;
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
